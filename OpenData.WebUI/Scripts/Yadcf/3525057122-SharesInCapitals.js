$(document).ready(function(){var oTable=$('#datatable').dataTable({'iDisplayLength': 10,'bJQueryUI': true,'bProcessing': true,'bServerSide': false,'sPaginationType': 'two_button','oLanguage':{sUrl:'/Content/datatables_lang_rus.txt'},'sAjaxSource': '/Table/GetTable?ODID=3525057122-SharesInCapitals','aoColumns':[{'sTitle':'№ п/п'},{'sTitle':'Реестр. №'},{'sTitle':'Наименование хозяйственного общества (эмитента)'},{'sTitle':'Почтовый адрес'},{'sTitle':'Уставный капитал, тыс.руб.'},{'sTitle':'Доля области, %'},{'sTitle':'Акции в соб-сти области, шт.'},{'sTitle':'Примечание'}],'sDom':'C<"clear">lfrtip','oColVis':{'buttonText':'Показать/скрыть'}}).columnFilter({'sPlaceHolder': 'thead:after', 'aoColumns':[{type:'text'},{type:'text'},{type:'text'},{type:'text'},{type:'text'},{type:'text'},{type:'text'},{type:'text'}]}); $('#datatable').on('dblclick','td',function (e) {var sData = oTable.fnGetPosition( this );$('#requestCreation').show();$('#requestCreation #row').text(sData[0]);  }); $('#requestCreation .cancelButton').click(function() {$('#requestCreation').hide()}); });