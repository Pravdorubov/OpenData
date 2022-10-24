/// <reference path="demo.core.js" />
/// <reference path="../Scripts/jquery-1.8.1.js" />
/// <reference path="../Scripts/knockout-2.1.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.js" />





///////////////////////////////////////////////////////////////
//  site.feedback
//  Работает через dataService с объектами на форме
//  Feedback
//  автор: calabonga.net
///////////////////////////////////////////////////////////////

(function (site) {

    "use strict";

    site.vm.feedbackViewModel = function(){
        var
            view = {
                title: "Отправка сообщения"              
            },
            subjects = ko.observableArray([]),
            isbusy = ko.observable(false),
            loadSubjects = function () {
                isbusy(true);
                site.services.feedbackForm.loadSubjects(callback);
            },
            callback = function (json) {
                isbusy(false);
                ko.mapping.fromJS(json, {}, subjects);
                //subjects(json);
                var total = subjects().length;
            }

        loadSubjects();

        return {
            view: view,
            subjects: subjects,
            isbusy: isbusy
        }
            
    }();

})(site);



$(function () {

    "use strict";

    // привязка ViewModel к форме
    ko.applyBindings(site.vm.feedbackViewModel);
});