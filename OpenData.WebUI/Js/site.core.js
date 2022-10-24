/// <reference path="../Scripts/jquery-1.8.1.js" />
/// <reference path="../Scripts/knockout-2.1.0.debug.js" />

///////////////////////////////////////////////////////////////
// создаем свой namespace
//  зависит от:     
//  автор: calabonga.net
///////////////////////////////////////////////////////////////

var site = site || {};





///////////////////////////////////////////////////////////////
// модуль сервисов
//  зависит от:     site.js
//  автор: calabonga.net
///////////////////////////////////////////////////////////////

site.services = site.services || {};





///////////////////////////////////////////////////////////////
// модуль с ViewModels
//  зависит от:     site.js
//  автор: calabonga.net
///////////////////////////////////////////////////////////////

site.vm = site.vm || {};





///////////////////////////////////////////////////////////////
//  DataServices = обертка на JQuery.getJSON и postJSON
//  зависит от:     site.js
//                  jquery.js
//  автор: calabonga.net
///////////////////////////////////////////////////////////////

(function (site) {

    "use strict";

    var baseUrl = "/ajax/",
        serviceUrl = function (method) { return baseUrl + method; };

    site.services.ajax = function () {
        var getAjaxJson = function (method, jsonIn, callback) {
            $.ajax({
                url: serviceUrl(method),
                data: ko.toJS(jsonIn),
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (json) {
                    callback(json);
                },
                error: function (jqXHR, textStatus) {
                    if (confirm(jqXHR.status + " "
                        + textStatus
                        + ":"
                        + jqXHR.statusText)) {
                        alert(jqXHR.responseText);
                    }
                }
            });
        },
        postAjaxJson = function (method, jsonIn, callback) {
            $.ajax({
                url: serviceUrl(method),
                data: ko.toJS(jsonIn),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (json) {
                    callback(json);
                },
                error: function (jqXHR, textStatus) {
                    if (confirm(jqXHR.status
                        + " "
                        + textStatus
                        + ":"
                        + jqXHR.statusText)) {
                        alert(jqXHR.responseText);
                    }
                }
            });
        }
        return {
            get: getAjaxJson,
            post: postAjaxJson
        };
    }();

})(site);