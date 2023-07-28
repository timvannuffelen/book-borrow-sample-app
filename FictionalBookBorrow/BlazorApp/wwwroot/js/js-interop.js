/**
 * Store the value of the cookieString parameter in a cookie. This value is set in the CookieConsentBanner component. 
 */
window.CookieFunction = {
    acceptMessage: function (cookieString) {
        document.cookie = cookieString;
    }
};