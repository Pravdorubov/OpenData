$(document).ready(function(){$('#userTable').dataTable({'sDom':'rt','bJQueryUI':true,'sPaginationType':'full_numbers','bProcessing':true,'bServerSide':true,'sAjaxSource':'/Table/GetDataTables','fnServerData':function(url,data,callback){$.ajax({'url':url,'data':data,'success':callback,'dataType':'json','type':'POST','cache':false,'error':function(){alert('DataTableswarning:JSONdatafromserverfailedtoloadorbeparsed.'+'ThisismostlikelytobecausedbyaJSONformattingerror.');}});}});});