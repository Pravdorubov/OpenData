ko.bindingHandlers.fadeVisible = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(), allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        var duration = allBindings.fadeDuration || 200;
        if (valueUnwrapped == true)
            $(element).fadeIn(duration); // Make the element visible
        else
            $(element).hide(); // Make the element invisible
    }
};

ko.bindingHandlers.autocomplete = {
    init: function (element, valueAccessor) {
        var options = valueAccessor(),
            dataCallback = options.source;

        $(element).autocomplete({
            source: function (req, res) { dataCallback(req.term, res) },
            select: function (event, ui) {
                var res = ui.item.data || ui.item.value || ui.item;
                if (options.select) {
                    options.select(res);
                    $(element).val("");
                } else {
                    $(element).val(res);
                }
                $(element).change();
                event.preventDefault();
            }
        });
    }
};

ko.bindingHandlers.ko_autocomplete = {
    init: function (element, params) {
        $(element).autocomplete(params());
    },
    update: function (element, params) {
        $(element).autocomplete("option", "source", params().source);
    }
};

ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {};
        ko.utils.extend(options, {
            dateFormat: "dd.mm.yy",
            dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
            firstDay: 1,
            monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октярь", "Ноябрь", "Декабрь"],
            altFormat: "dd.mm.yy"
        });
        $(element).datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker("destroy");
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            current = $(element).datepicker("getDate");

        if (value - current !== 0) {
            $(element).datepicker("setDate", value);
        }

    }
};

ko.extenders.required = function (target, overrideMessage) {
    overrideMessage = typeof overrideMessage == 'string' ? overrideMessage : "Заполните поле";
    target.hasError = ko.observable();
    target.validationMessage = ko.observable();

    target.validate = function (newValue) {
        var valid = newValue ? false : target() ? false : true;
        target.hasError(valid);
        target.validationMessage(!valid ? "" : overrideMessage || "Не заполнено обязательное поле.");
        return valid;
    };

    target.subscribe(target.validate);
    return target;
};

ko.bindingHandlers.dateString = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        var pattern = allBindings.datePattern || 'DD.MM.YYYY';
        $(element).text(moment(valueUnwrapped) ? moment(valueUnwrapped).format(pattern) : "");
    }
};

ko.bindingHandlers.newDateString = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        var pattern = allBindings.datePattern || 'HH:mm';
        $(element).text(moment(valueUnwrapped).format(pattern));
    }
};

ko.dirtyFlag = function (root, isInitiallyDirty) {
    var result = function () { };
    var _initialState = ko.observable(ko.toJSON(root));
    var _isInitiallyDirty = ko.observable(!!isInitiallyDirty);

    result.isDirty = ko.computed(function () {
        return _isInitiallyDirty() || _initialState() !== ko.toJSON(root);
    });
    result.save = function () {
        _initialState(ko.toJSON(root));
        _isInitiallyDirty(false);
    };
    result.reset = function () {
        _isInitiallyDirty(false);
        return JSON.parse(_initialState());
    };

    return result;
};

ko.lazyObservableArray = function (callback, target) {
    var _value = ko.observable();  //private observable

    var result = ko.dependentObservable({
        read: function () {
            //if it has not been loaded, execute the supplied function
            if (!result.loaded()) {
                callback.call(target);
            }
            //always return the current value
            return _value();
        },
        write: function (newValue) {
            //indicate that the value is now loaded and set it
            result.loaded(true);
            _value(newValue);
        },
        deferEvaluation: true  //do not evaluate immediately when created
    });

    //expose the current state, which can be bound against
    result.loaded = ko.observable();
    //load it again
    result.refresh = function () {
        result.loaded(false);
    };

    return result;
};

var windowURL = window.URL || window.webkitURL;
ko.bindingHandlers.file = {
    init: function (element, valueAccessor) {
        $(element).change(function () {
            var file = this.files[0];
            if (file.type == "image/png") {
                if (ko.isObservable(valueAccessor())) {
                    valueAccessor()(file);
                }
            } else {
                alert("Изображение не будет сохранено, так как допустимый формат только .png");
            }
        });
    },

    update: function (element, valueAccessor, allBindingsAccessor) {
        var file = ko.utils.unwrapObservable(valueAccessor());
        var bindings = allBindingsAccessor();

        if (bindings.fileObjectURL && ko.isObservable(bindings.fileObjectURL)) {
            var oldUrl = bindings.fileObjectURL();
            if (oldUrl) {
                windowURL.revokeObjectURL(oldUrl);
            }
            bindings.fileObjectURL(file && windowURL.createObjectURL(file));
        }

        if (bindings.fileBinaryData && ko.isObservable(bindings.fileBinaryData)) {
            if (!file) {
                bindings.fileBinaryData(null);
            } else {
                var reader = new FileReader();
                reader.onload = function (e) {
                    bindings.fileBinaryData(e.target.result);
                };
                reader.readAsArrayBuffer(file);
            }
        }
        if (bindings.fileBinaryData && ko.isObservable(bindings.fileBinaryData)) {
                    if (!file) {
                        bindings.fileBinaryData(null);
                    } else {
                        var reader2 = new FileReader();
                        reader2.onload = function (e) {

                            bindings.fileBase64Data(e.target.result.substr(22));//пропускаем data:image/png;base64,
                        };
                        reader2.readAsDataURL(file);
                    }
                }
    }
};


ko.fullCalendar = {
    // Defines a view model class you can use to populate a calendar
    viewModel: function (configuration) {
        this.events = configuration.events;
        this.firstDay = 1;
        this.header = {
            left: 'title',
            center: '',
            right: 'prevYear,prev,next,nextYear today'
        };
        this.monthNames = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];
        this.monthNamesShort = ['Янв.', 'Фев.', 'Март', 'Апр.', 'Май', 'Июнь', 'Июль', 'Авг.', 'Сент.', 'Окт.', 'Ноя.', 'Дек.'];
        this.dayNames = ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"];
        this.dayNamesShort = ["ВС", "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ"];
        this.buttonText = {
            today: "Сегодня",
            month: "Месяц",
            week: "Неделя",
            day: "День"
        };
        this.editable = configuration.editable;
        this.viewDate = configuration.viewDate || ko.observable(new Date());
        this.eventRender = function(event, element) {
            var descriptionDiv = document.createElement('div');
            descriptionDiv.className = 'fc-event-description';
            element[0].appendChild(descriptionDiv);
            $(element[0]).addClass(event.forClass);
            descriptionDiv.innerHTML = event.description;
            //console.log(element[0]);
        };
        this.eventClick = configuration.eventClick;
    }
};

// The "fullCalendar" binding
ko.bindingHandlers.fullCalendar = {
    init: function (element, viewModelAccessor, allBindingsAccessor) {
        var viewModel = viewModelAccessor();
        var bindings = allBindingsAccessor();
        element.innerHTML = "";
        $(element).fullCalendar({
            firstDay: 1,
            events: function(start, end, callback) {
                callback(ko.utils.unwrapObservable(viewModel.events));
            },
            header: viewModel.header,
            editable: viewModel.editable,
            monthNames: viewModel.monthNames,
            monthNamesShort: viewModel.monthNamesShort,
            dayNames: viewModel.dayNames,
            dayNamesShort: viewModel.dayNamesShort,
            buttonText: viewModel.buttonText,
            viewDate: viewModel.viewDate,
            eventRender: viewModel.eventRender,
            eventClick: viewModel.eventClick,
        });
        
        $(".fc-button").click(bindings.fullCalendarButtonClick);
    },
    update: function (element, viewModelAccessor, allBindingsAccessor) {
        var viewModel = viewModelAccessor();
        var bindings = allBindingsAccessor();
        viewModel.events();
        //$(element).fullCalendar({
        //    firstDay: 1,
        //    events: ko.utils.unwrapObservable(viewModel.events),
        //    header: viewModel.header,
        //    editable: viewModel.editable,
        //    monthNames: viewModel.monthNames,
        //    monthNamesShort: viewModel.monthNamesShort,
        //    dayNames: viewModel.dayNames,
        //    dayNamesShort: viewModel.dayNamesShort,
        //    buttonText: viewModel.buttonText,
        //    viewDate: viewModel.viewDate,
        //    eventRender: viewModel.eventRender,
        //    eventClick: viewModel.eventClick,
        //});
        //$(element).fullCalendar('gotoDate', ko.utils.unwrapObservable(viewModel.viewDate));
        $(element).fullCalendar('refetchEvents');
    }
};