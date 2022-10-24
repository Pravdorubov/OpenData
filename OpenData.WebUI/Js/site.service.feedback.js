/// <reference path="demo.core.js" />
/// <reference path="../Scripts/jquery-1.8.1.js" />
/// <reference path="../Scripts/knockout-2.1.0.debug.js" />




///////////////////////////////////////////////////////////////
//  site.feedback
//  Работает через dataService с объектами на форме
//  Feedback
//  автор: calabonga.net
///////////////////////////////////////////////////////////////

(function (site) {

    "use strict";

    site.services.feedbackForm = {
        loadSubjects: function (callback) {
            if (typeof callback === undefined) {
                throw new Error(200, "callback is undefined");
            }
            site.services.ajax.get("LoadSubjects", {}, callback);
        },
        sendFeedback: function (feedback, callback) {
            if (typeof callback === undefined) {
                throw new Error(200, "callback is undefined");
            }
            site.services.ajax.post("SendFeedback", feedback, callback);
        }
    };

})(site);