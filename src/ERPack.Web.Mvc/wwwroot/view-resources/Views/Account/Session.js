$(document).ready(function () {
    function handleSessionExpiration() {
        $.get("/Account/IsAuthenticated", function (response) {
            if (!response.isAuthenticated) {
                if (confirm("Your session has expired. Please log in again to continue.")) {
                    window.location.href = "/Account/Login"; 
                }
            }
        });
    }
    var sessionTimeoutMilliseconds = 30 * 60 * 1000; 
    setInterval(handleSessionExpiration, sessionTimeoutMilliseconds);
});