using Abp.AspNetCore.Mvc.Authorization;
using Abp.Logging;
using DocumentFormat.OpenXml.Wordprocessing;
using ERPack.Authorization;
using ERPack.Categories;
using ERPack.Categories.Dto;
using ERPack.Common;
using ERPack.Controllers;
using ERPack.Customers;
using ERPack.Customers.Dto;
using ERPack.Preferences;
using ERPack.Web.Models.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Customers)]
    public class CustomersController : ERPackControllerBase
    {
        private readonly ICustomerAppService _customerAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly ICurrencyAppService _currencyAppService;
        private readonly ICountryMasterAppService _countryMasterAppService;
        private readonly IStateMasterAppService _stateMasterAppService;
        private readonly IPreferenceAppService _preferenceAppService;
        private readonly ICurrencyMasterAppService _currencyMasterAppService;


        public CustomersController(ICustomerAppService customerAppService,
            ICategoryAppService categoryAppService,
            ICurrencyAppService currencyAppService,
            ICountryMasterAppService countryMasterAppService,
            IStateMasterAppService stateMasterAppService,
            IPreferenceAppService preferenceAppService,
            ICurrencyMasterAppService currencyMasterAppService)
        {
            _customerAppService = customerAppService;
            _categoryAppService = categoryAppService;
            _currencyAppService = currencyAppService;
            _countryMasterAppService = countryMasterAppService;
            _stateMasterAppService = stateMasterAppService;
            _preferenceAppService = preferenceAppService;
            _currencyMasterAppService = currencyMasterAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CreateCustomer(long customerId = 0, string returnUrl = "")
        {
            AddEditCustomerModel model = new AddEditCustomerModel();
            if (customerId > 0)
            {
                CustomerDto customer = await _customerAppService.GetAsync(customerId);
                model = ObjectMapper.Map<AddEditCustomerModel>(customer);
            }
            else
            {
                model.CustomerId = await _preferenceAppService.GetByNameAsync("CustomerId");
            }
            model.Categories = await _categoryAppService.GetAllAsync();
            model.BusinessCurrencies = await _currencyAppService.GetAllAsync();
            model.States = await _stateMasterAppService.GetStatesAsync();
            model.Countries = await _countryMasterAppService.GetCountriesAsync();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> CreateCustomer(AddEditCustomerModel input)
        {
            var errMsg = string.Empty;
            try
            {
                CustomerDto customerDto = ObjectMapper.Map<CustomerDto>(input);
                customerDto.TenantId = AbpSession.TenantId;

                if (input.Id == 0)
                {
                    (var customerId, errMsg) = await _customerAppService.CreateAsync(customerDto);
                    return Json(new
                    {
                        msg = errMsg,
                        id = customerId
                    });
                }
                else
                {
                    (var customer, errMsg) = await _customerAppService.UpdateAsync(customerDto);
                    return Json(new
                    {
                        msg = errMsg,
                        id = customer.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error in creating customer", ex);
                return Json(new
                {
                    msg = errMsg,
                    id = 0
                });
            }
        }

        public async Task<JsonResult> GetCustomersNames(string name)
        {
            var customers = await _customerAppService.GetCustomersNamesAsync(name);
            if (customers != null)
            {
                string jsonData = JsonConvert.SerializeObject(customers);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });

            }
            else
            {
                Logger.Log(LogSeverity.Warn, "Not able to found cusotmers");
                return Json(new
                {
                    msg = "ERROR"
                });
            }
        }

        public async Task<JsonResult> GetCustomerById(long id)
        {
            var customer = await _customerAppService.GetByIdAsync(id);
            if (customer == null)
            {
                return Json(new
                {
                    msg = "ERROR"
                });
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(customer);
                return Json(new
                {
                    msg = "OK",
                    data = jsonData
                });

            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportCustomer(IFormFile importFile)
        {
            var errMsg = string.Empty;
            StringBuilder validationMessage = new StringBuilder();
            bool isCustomersImported = false;
            try
            {
                if (importFile == null || importFile.Length <= 0)
                {
                    return Json(new { isValid = false, msg = "Please select file to import customers" });
                }

                string fileExtension = Path.GetExtension(importFile.FileName);
                if (!fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) && !fileExtension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { isValid = false, msg = "Please select only excel or csv files" });
                }

                using (var stream = new MemoryStream())
                {
                    if (fileExtension.Equals(".xlsx"))
                    {
                        await importFile.CopyToAsync(stream);

                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                            var rowCount = worksheet.Dimension.Rows;
                            if (rowCount < 2)
                            {
                                return Json(new { isValid = false, msg = "Please Insert some data in excel file" });
                            }

                            if (rowCount > 0)
                            {
                                int customerNameCol = -1;
                                int customerEmailCol = -1;
                                int industryCol = -1;
                                int categoryCol = -1;
                                int websiteCol = -1;
                                int countryCol = -1;
                                int panNoCol = -1;
                                int currencyCol = -1;
                                //int dAddressNameCol = -1;
                                //int dAddressLine1Col = -1;
                                //int dAddressLine2Col = -1;
                                int dCityCol = -1;
                                int dStateCol = -1;
                                //int dCountryCol = -1;
                                int dPinCodeCol = -1;
                                int gstNoCol = -1;

                                var allCategries = await _categoryAppService.GetAllAsync();
                                if (allCategries == null || allCategries.Count() == 0)
                                {
                                    return Json(new { isValid = false, msg = "", NoCategoryErrorMessage = "There is no categories in the system for this tenant, No customer data is imported" });
                                }
                                int categoryId = 0;
                                var currencies = await _currencyMasterAppService.GetCurrenciesAsync();
                                int currencyId = 0;
                                var countries = await _countryMasterAppService.GetCountriesAsync();
                                int countryId = 0;
                                var states = await _stateMasterAppService.GetStatesAsync();
                                int stateId = 0;

                                for (int i = 1; i < worksheet.Cells.Columns; i++)
                                {
                                    if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "CUSTOMER NAME")
                                        customerNameCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "CUSTOMER EMAIL")
                                        customerEmailCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "INDUSTRY")
                                        industryCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "CATEGORY")
                                        categoryCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "WEBSITE")
                                        websiteCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "COUNTRY")
                                        countryCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "PAN NUMBER")
                                        panNoCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "CURRENCY")
                                        currencyCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "DEFAULT CITY")
                                        dCityCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "DEFAULT STATE")
                                        dStateCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "DEFAULT PINCODE")
                                        dPinCodeCol = i;
                                    else if (worksheet.Cells[1, i].Text.ToUpper().Trim() == "GST NUMBER")
                                        gstNoCol = i;

                                    if (customerNameCol > -1 && customerEmailCol > -1 && industryCol > -1 && categoryCol > -1 && websiteCol > -1 && countryCol > -1 && panNoCol > -1 && currencyCol > -1 && /*dAddressNameCol > -1 && dAddressLine1Col > -1 && dAddressLine2Col > -1 &&*/ dCityCol > -1 && dStateCol > -1 && /*dCountryCol > -1 &&*/ dPinCodeCol > -1 && gstNoCol > -1)
                                        break;
                                }
                                if (customerNameCol == -1 || customerEmailCol == -1 || industryCol == -1 || categoryCol == -1 || websiteCol == -1 || countryCol == -1 || panNoCol == -1 || currencyCol == -1 || /*dAddressNameCol == -1 || dAddressLine1Col == -1 || dAddressLine2Col == -1 ||*/ dCityCol == -1 || dStateCol == -1 || /*dCountryCol == -1 ||*/ dPinCodeCol == -1 || gstNoCol == -1)
                                {
                                    return Json(new { isValid = false, msg = "Please verify all columns of uploaded file with sample file" });
                                }

                                for (int row = 2; row <= rowCount; row++)
                                {
                                    //--------------------------- Add Fields Here -----------------------------------
                                    string customerName = Convert.ToString(worksheet.Cells[row, customerNameCol].Text).Trim();
                                    string customerEmail = Convert.ToString(worksheet.Cells[row, customerEmailCol].Text).Trim();
                                    string industry = Convert.ToString(worksheet.Cells[row, industryCol].Text).Trim();
                                    string categoryName = Convert.ToString(worksheet.Cells[row, categoryCol].Text).Trim();
                                    string website = Convert.ToString(worksheet.Cells[row, websiteCol].Text).Trim();
                                    string country = Convert.ToString(worksheet.Cells[row, countryCol].Text).Trim();
                                    string panNo = Convert.ToString(worksheet.Cells[row, panNoCol].Text).Trim();
                                    string currencyName = Convert.ToString(worksheet.Cells[row, currencyCol].Text).Trim();
                                    string dCity = Convert.ToString(worksheet.Cells[row, dCityCol].Text).Trim();
                                    string dState = Convert.ToString(worksheet.Cells[row, dStateCol].Text).Trim();
                                    string dPinCode = Convert.ToString(worksheet.Cells[row, dPinCodeCol].Text).Trim();
                                    string gstNo = Convert.ToString(worksheet.Cells[row, gstNoCol].Text).Trim();

                                    //---------------------- Fields Validation here --------------------------------
                                    (string message, bool isContinue) = ValidateEmptyFields(customerName, customerEmail, currencyName, dCity, dState, dPinCode);
                                    if (isContinue)
                                    {
                                        validationMessage.Append($"{message} {row} <br />");
                                        continue;
                                    }
                                    if (!string.IsNullOrEmpty(categoryName))
                                    {
                                        if (allCategries != null && allCategries.Count() > 0)
                                        {
                                            categoryId = allCategries.Where(x => x.CategoryName.ToUpper().Equals(categoryName.ToUpper())).Select(x => x.Id).FirstOrDefault();
                                            if (categoryId == 0)
                                            {
                                                validationMessage.Append($"Category Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                        //To insert the category namewhile importing customer data
                                        //else
                                        //{
                                        //    CategoryDto categoryDto = new();
                                        //    {
                                        //        categoryDto.CategoryName = categoryName;
                                        //        categoryDto.TenantId = AbpSession.TenantId;
                                        //        categoryDto.CreatorUserId = AbpSession.UserId;
                                        //    }

                                        //    (var newCategoryId, var newErrMsg) = await _categoryAppService.CreateCategoryAsync(categoryDto);
                                        //    categoryId = (int)newCategoryId;
                                        //    allCategries = await _categoryAppService.GetAllAsync();
                                        //}
                                    }
                                    if (!string.IsNullOrEmpty(currencyName))
                                    {
                                        if (currencies != null && currencies.Count() > 0)
                                        {
                                            currencyId = currencies.Where(x => x.Name.ToUpper().Equals(currencyName.ToUpper())).Select(x => x.Id).FirstOrDefault();
                                            if (currencyId == 0)
                                            {
                                                validationMessage.Append($"Currency Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(country))
                                    {
                                        if (countries != null && countries.Count() > 0)
                                        {
                                            countryId = countries.Where(x => x.CountryName.ToUpper().Equals(country.ToUpper())).Select(x => x.CountryId).FirstOrDefault();
                                            if (countryId == 0)
                                            {
                                                validationMessage.Append($"Country Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(dState))
                                    {
                                        if (states != null && states.Count() > 0)
                                        {
                                            stateId = states.Where(x => x.StateName.ToUpper().Equals(dState.ToUpper())).Select(x => x.StateId).FirstOrDefault();
                                            if (stateId == 0)
                                            {
                                                validationMessage.Append($"State Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(panNo))
                                    {
                                        var allCustomersList = (await _customerAppService.GetCustomersListAsync()).Items;
                                        if (allCustomersList != null && allCustomersList.Count() > 0)
                                        {
                                            allCustomersList = allCustomersList.Where(x => x.PAN != null).ToList();
                                            if (allCustomersList.Any(x => x.PAN.ToUpper().Equals(panNo.ToUpper())))
                                            {
                                                validationMessage.Append($"PAN number is already exists at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }

                                    (string exceptionMessage, string errMessage, bool isSuccess) = await InserCustomerData(customerName, customerEmail, industry, categoryId, website, countryId, panNo, dCity, stateId, dPinCode, gstNo, currencyId);
                                    if (isSuccess)
                                    {
                                        isCustomersImported = true;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(errMessage))
                                        {
                                            validationMessage.Append($"{errMessage} {row} <br />");
                                        }
                                        if (!string.IsNullOrEmpty(exceptionMessage))
                                        {
                                            validationMessage.Append($"{exceptionMessage} {row} <br />");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (fileExtension.Equals(".csv"))
                    {
                        await importFile.CopyToAsync(stream);
                        stream.Position = 0;

                        using (var reader = new StreamReader(stream))
                        {
                            var csvData = new List<string[]>();
                            while (!reader.EndOfStream)
                            {
                                var line = await reader.ReadLineAsync();
                                var values = line.Split(','); // Adjust the delimiter if necessary
                                csvData.Add(values);
                            }

                            if (csvData.Count < 2)
                            {
                                return Json(new { isValid = false, msg = "Please Insert some data in excel file" });
                            }

                            var headers = csvData[0];

                            int customerNameCol = Array.IndexOf(headers, "CUSTOMER NAME");
                            int customerEmailCol = Array.IndexOf(headers, "CUSTOMER EMAIL");
                            int industryCol = Array.IndexOf(headers, "INDUSTRY");
                            int categoryCol = Array.IndexOf(headers, "CATEGORY");
                            int websiteCol = Array.IndexOf(headers, "WEBSITE");
                            int countryCol = Array.IndexOf(headers, "COUNTRY");
                            int panNoCol = Array.IndexOf(headers, "PAN NUMBER");
                            int currencyCol = Array.IndexOf(headers, "CURRENCY");
                            //int dAddressNameCol = Array.IndexOf(headers, "DEFAULT ADDRESS NAME");
                            //int dAddressLine1Col = Array.IndexOf(headers, "DEFAULT ADDRESS LINE 1");
                            //int dAddressLine2Col = Array.IndexOf(headers, "DEFAULT ADDRESS LINE 2");
                            int dCityCol = Array.IndexOf(headers, "DEFAULT CITY");
                            int dStateCol = Array.IndexOf(headers, "DEFAULT STATE");
                            //int dCountryCol = Array.IndexOf(headers, "DEFAULT COUNTRY");
                            int dPinCodeCol = Array.IndexOf(headers, "DEFAULT PINCODE");
                            int gstNoCol = Array.IndexOf(headers, "GST NUMBER");

                            if (customerNameCol == -1 || customerEmailCol == -1 || industryCol == -1 || categoryCol == -1 || websiteCol == -1 || countryCol == -1 || panNoCol == -1 || currencyCol == -1 || /*dAddressNameCol == -1 || dAddressLine1Col == -1 || dAddressLine2Col == -1 ||*/ dCityCol == -1 || dStateCol == -1 || /*dCountryCol == -1 ||*/ dPinCodeCol == -1 || gstNoCol == -1)
                            {
                                return Json(new { isValid = false, msg = "Please verify all columns of uploaded file with sample file" });
                            }

                            var allCategries = await _categoryAppService.GetAllAsync();
                            if (allCategries == null || allCategries.Count() == 0)
                            {
                                return Json(new { isValid = false, msg = "", NoCategoryErrorMessage = "There is no categories in the system for this tenant, No customer data is imported" });
                            }
                            int categoryId = 0;
                            var currencies = await _currencyMasterAppService.GetCurrenciesAsync();
                            int currencyId = 0;
                            var countries = await _countryMasterAppService.GetCountriesAsync();
                            int countryId = 0;
                            var states = await _stateMasterAppService.GetStatesAsync();
                            int stateId = 0;

                            if (customerNameCol > -1 && customerEmailCol > -1 && industryCol > -1 && categoryCol > -1 && websiteCol > -1 && countryCol > -1 && panNoCol > -1 && currencyCol > -1 && /*dAddressNameCol > -1 && dAddressLine1Col > -1 && dAddressLine2Col > -1 &&*/ dCityCol > -1 && dStateCol > -1 && /*dCountryCol > -1 &&*/ dPinCodeCol > -1 && gstNoCol > -1)
                            {
                                for (int row = 1; row < csvData.Count; row++)
                                {
                                    var values = csvData[row];
                                    //--------------------------- Add Fields Here -----------------------------------
                                    string customerName = customerNameCol >= 0 && customerNameCol < values.Length ? values[customerNameCol].Trim() : string.Empty;
                                    string customerEmail = customerEmailCol >= 0 && customerEmailCol < values.Length ? values[customerEmailCol].Trim() : string.Empty;
                                    string industry = industryCol >= 0 && industryCol < values.Length ? values[industryCol].Trim() : string.Empty;
                                    string categoryName = categoryCol >= 0 && categoryCol < values.Length ? values[categoryCol].Trim() : string.Empty;
                                    string website = websiteCol >= 0 && websiteCol < values.Length ? values[websiteCol].Trim() : string.Empty;
                                    string country = countryCol >= 0 && countryCol < values.Length ? values[countryCol].Trim() : string.Empty;
                                    string panNo = panNoCol >= 0 && panNoCol < values.Length ? values[panNoCol].Trim() : string.Empty;
                                    string currencyName = currencyCol >= 0 && currencyCol < values.Length ? values[currencyCol].Trim() : string.Empty;
                                    //string dAddressName = dAddressNameCol >= 0 && dAddressNameCol < values.Length ? values[dAddressNameCol].Trim() : string.Empty;
                                    //string dAddressLine1 = dAddressLine1Col >= 0 && dAddressLine1Col < values.Length ? values[dAddressLine1Col].Trim() : string.Empty;
                                    //string dAddressLine2 = dAddressLine2Col >= 0 && dAddressLine2Col < values.Length ? values[dAddressLine2Col].Trim() : string.Empty;
                                    string dCity = dCityCol >= 0 && dCityCol < values.Length ? values[dCityCol].Trim() : string.Empty;
                                    string dState = dStateCol >= 0 && dStateCol < values.Length ? values[dStateCol].Trim() : string.Empty;
                                    //string dCountry = dCountryCol >= 0 && dCountryCol < values.Length ? values[dCountryCol].Trim() : string.Empty;
                                    string dPinCode = dPinCodeCol >= 0 && dPinCodeCol < values.Length ? values[dPinCodeCol].Trim() : string.Empty;
                                    string gstNo = gstNoCol >= 0 && gstNoCol < values.Length ? values[gstNoCol].Trim() : string.Empty;

                                    //---------------------- Fields Validation here --------------------------------
                                    (string message, bool isContinue) = ValidateEmptyFields(customerName, customerEmail, currencyName, dCity, dState, dPinCode);
                                    if (isContinue)
                                    {
                                        validationMessage.Append($"{message} {row} <br />");
                                        continue;
                                    }
                                    if (!string.IsNullOrEmpty(categoryName))
                                    {
                                        if (allCategries != null && allCategries.Count() > 0)
                                        {
                                            categoryId = allCategries.Where(x => x.CategoryName.ToUpper().Equals(categoryName.ToUpper())).Select(x => x.Id).FirstOrDefault();
                                            if (categoryId == 0)
                                            {
                                                validationMessage.Append($"Category Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                        //To insert the category namewhile importing customer data
                                        //else
                                        //{
                                        //    CategoryDto categoryDto = new();
                                        //    {
                                        //        categoryDto.CategoryName = categoryName;
                                        //        categoryDto.TenantId = AbpSession.TenantId;
                                        //        categoryDto.CreatorUserId = AbpSession.UserId;
                                        //    }

                                        //    (var newCategoryId, var newErrMsg) = await _categoryAppService.CreateCategoryAsync(categoryDto);
                                        //    categoryId = (int)newCategoryId;
                                        //    allCategries = await _categoryAppService.GetAllAsync();
                                        //}
                                    }
                                    if (!string.IsNullOrEmpty(currencyName))
                                    {
                                        if (currencies != null && currencies.Count() > 0)
                                        {
                                            currencyId = currencies.Where(x => x.Name.ToUpper().Equals(currencyName.ToUpper())).Select(x => x.Id).FirstOrDefault();
                                            if (currencyId == 0)
                                            {
                                                validationMessage.Append($"Currency Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(country))
                                    {
                                        if (countries != null && countries.Count() > 0)
                                        {
                                            countryId = countries.Where(x => x.CountryName.ToUpper().Equals(country.ToUpper())).Select(x => x.CountryId).FirstOrDefault();
                                            if (countryId == 0)
                                            {
                                                validationMessage.Append($"Country Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(dState))
                                    {
                                        if (states != null && states.Count() > 0)
                                        {
                                            stateId = states.Where(x => x.StateName.ToUpper().Equals(dState.ToUpper())).Select(x => x.StateId).FirstOrDefault();
                                            if (stateId == 0)
                                            {
                                                validationMessage.Append($"State Name is invalid at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(panNo))
                                    {
                                        var allCustomersList = (await _customerAppService.GetCustomersListAsync()).Items;
                                        if (allCustomersList != null && allCustomersList.Count() > 0)
                                        {
                                            allCustomersList = allCustomersList.Where(x => x.PAN != null).ToList();
                                            if (allCustomersList.Any(x => x.PAN.ToUpper().Equals(panNo.ToUpper())))
                                            {
                                                validationMessage.Append($"PAN number is already exists at row: {row} <br /> ");
                                                continue;
                                            }
                                        }
                                    }

                                    (string exceptionMessage, string errMessage, bool isSuccess) = await InserCustomerData(customerName, customerEmail, industry, categoryId, website, countryId, panNo, dCity, stateId, dPinCode, gstNo, currencyId);
                                    if (isSuccess)
                                    {
                                        isCustomersImported = true;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(errMessage))
                                        {
                                            validationMessage.Append($"{errMessage} {row} <br />");
                                        }
                                        if (!string.IsNullOrEmpty(exceptionMessage))
                                        {
                                            validationMessage.Append($"{exceptionMessage} {row} <br />");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(validationMessage.ToString()))
                    validationMessage.Append("<br /> <b>Above customers data are not inserted.</b>");
                if (isCustomersImported)
                    return Json(new { isValid = true, msg = "", ValidationMessage = validationMessage });
                else
                    return Json(new { isValid = false, msg = "", ValidationMessage = validationMessage });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    isValid = false,
                    msg = "Something Went wrong while importing customer data",
                });
            }
        }

        #region Contact

        public async Task<ActionResult> CreateContact(long contactId = 0)
        {
            AddEditContactModel model = new AddEditContactModel();
            if (contactId > 0)
            {
                ContactDto customer = await _customerAppService.GetContactAsync(contactId);
                model = ObjectMapper.Map<AddEditContactModel>(customer);
            }
            return PartialView("_CreateContactModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(AddEditContactModel input)
        {
            try
            {
                ContactDto contactDto = ObjectMapper.Map<ContactDto>(input);
                contactDto.TenantId = AbpSession.TenantId;

                if (input.Id == 0)
                {
                    contactDto.TenantId = AbpSession.TenantId;
                    var contactId = await _customerAppService.CreateContactAsync(contactDto);
                    if (contactId == 0)
                    {
                        return Json(new
                        {
                            msg = "ERROR",
                            id = 0
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            msg = "OK",
                            id = contactId
                        });
                    }
                }
                else
                {
                    var contact = await _customerAppService.UpdateContactAsync(contactDto);
                    return Json(new
                    {
                        msg = "OK",
                        id = contact.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error in creating contact", ex);
                return Json(new
                {
                    msg = ex.Message,
                    id = 0
                });
            }
        }

        #endregion

        #region Taxation Info

        public async Task<IActionResult> CreateCustomerTaxation(long taxationId = 0)
        {
            AddEditTaxationInfoModel model = new AddEditTaxationInfoModel();
            if (taxationId > 0)
            {
                CustomerTaxationInfoDto customer = await _customerAppService.GetTaxationAsync(taxationId);
                model = ObjectMapper.Map<AddEditTaxationInfoModel>(customer);
            }
            var States = await _stateMasterAppService.GetStatesAsync();
            model.States = States;

            var countries = await _countryMasterAppService.GetCountriesAsync();
            model.Countries = countries;
            return PartialView("_CreateCustomerTaxation", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerTaxation(AddEditTaxationInfoModel input)
        {
            try
            {
                CustomerTaxationInfoDto taxationDto = ObjectMapper.Map<CustomerTaxationInfoDto>(input);
                taxationDto.TenantId = AbpSession.TenantId;

                if (input.Id == 0)
                {
                    (var taxationId, var errMsg) = await _customerAppService.CreateTaxationAsync(taxationDto);
                    if (taxationId == 0)
                    {
                        return Json(new
                        {
                            msg = errMsg,
                            id = 0
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            msg = "OK",
                            id = taxationId
                        });
                    }
                }
                else
                {
                    var contact = await _customerAppService.UpdateTaxationAsync(taxationDto);
                    return Json(new
                    {
                        msg = "OK",
                        id = contact.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error in creating customer taxation", ex);
                return Json(new
                {
                    msg = ex.Message,
                    id = 0
                });
            }
        }

        #endregion

        public (string message, bool isContinue) ValidateEmptyFields(string customerName, string customerEmail, string currency, string dCity, string dState, string dPinCode)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                return ($"Customer Name is empty at row:", true);
            }
            if (string.IsNullOrEmpty(customerEmail))
            {
                return ($"Customer Email is empty at row:", true);
            }
            if (string.IsNullOrEmpty(currency))
            {
                return ($"Currency is empty at row:", true);
            }
            if (string.IsNullOrEmpty(dCity))
            {
                return ($"Default City is empty at row:", true);
            }
            if (string.IsNullOrEmpty(dState))
            {
                return ($"Default State is empty at row:", true);
            }
            if (string.IsNullOrEmpty(dPinCode))
            {
                return ($"Default Pin Code is empty at row:", true);
            }
            return (string.Empty, false);
        }

        public async Task<(string exceptionMessage, string errMessage, bool isCustomersImported)> InserCustomerData(string customerName, string customerEmail, string industry, int categoryId, string website, int countryId, string panNo, string dCity, int dStateId, string dPinCode, string gstNo, int currencyId)
        {
            string errMsg;
            AddEditCustomerModel addEditCustomerModel = new AddEditCustomerModel()
            {
                Name = customerName,
                EmailAddress = customerEmail,
                Industry = industry,
                CategoryId = categoryId,
                Website = website,
                CountryId = countryId,
                PAN = panNo,
                City = dCity,
                StateId = dStateId,
                PinCode = dPinCode,
                GSTNo = gstNo,
                BusinessCurrencyId = currencyId,
            };
            try
            {
                CustomerDto customerDto = ObjectMapper.Map<CustomerDto>(addEditCustomerModel);
                customerDto.CustomerId = await _preferenceAppService.GetByNameAsync("CustomerId", customerDto.Name);
                customerDto.TenantId = AbpSession.TenantId;

                (var customerId, errMsg) = await _customerAppService.CreateAsync(customerDto);
                if (customerId == 0)
                {
                    return (string.Empty, $"{errMsg} at row:", false);
                }
                else
                {
                    return (string.Empty, string.Empty, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, "Error in import customers", ex);
                return ($"Error on saving data at row:", string.Empty, false);
            }
        }
    }
}
