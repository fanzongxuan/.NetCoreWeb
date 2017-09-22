var $table = $("#email-account-grid");
var TableInit = function () {
    var oTable = new Object();
    oTable.QueryUrl = '/admin/EmailAccount/list' + '?rnd=' + +Math.random();
    oTable.Init = function () {
        $table.bootstrapTable({
            method: 'post',
            striped: true,      //是否显示行间隔色
            cache: false,      //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,     //是否显示分页（*）
            sortable: true,      //是否启用排序           
            pageNumber: 1,      //初始化加载第一页，默认第一页
            pageSize: 30,      //每页的记录行数（*）
            pageList: [30, 50, 100],  //可供选择的每页的行数（*）
            url: oTable.QueryUrl,//这个接口需要处理bootstrap table传递的固定参数
            queryParamsType: '', //默认值为 'limit' ,在默认情况下 传给服务端的参数为：offset,limit,sort
            // 设置为 '' 在这种情况下传给服务器的参数为：pageSize,pageNumber
            queryParams: oTable.queryParams,//前端调用服务时，会默认传递上边提到的参数，如果需要添加自定义参数，可以自定义一个函数返回请求参数
            sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
            showExport: true,                     //是否显示导出
            exportDataType: "basic",
            showRefresh: true,
            showToggle: true,
            showColumns: true,
            columns: [
                {
                    field: "email",
                    title: "Email address",
                    align: "center",
                    valign: "middle"
                },
                {
                    field: "displayName",
                    title: "Display name",
                    align: "center",
                    valign: "middle"
                },
                {
                    field: "host",
                    title: "Host",
                    align: "center",
                    valign: "middle"
                },
                {
                    field: "isDefaultEmailAccount",
                    title: "Is default email account",
                    align: "center",
                    valign: "middle",
                    formatter: function (value, row, index) {
                        var a;
                        if (value == true) {
                            a = '<i class="fa fa-check">'
                        } else {
                            a = '<i class="fa fa-check">'
                        }
                        return a;
                    },
                },
                {
                    field: "id",
                    title: "Operation",
                    align: "center",
                    valign: "middle",
                    formatter: function (value, row, index) {
                        var a;
                        if (value != null) {
                            a = '<a class="btn btn-info btn-xs"   href="/admin/EmailAccount/Update/' + value + '" >Edit</a>'
                        } else {
                            a = '-';
                        }
                        return a;
                    },
                    events: 'operateEvents'
                }
            ]
        });
    }
    oTable.queryParams = function (params) {
        var data = {
            PageSize: params.pageSize,
            Page: params.pageNumber,
        };
        return data;
    }

    return oTable;
}

window.operateEvents = {
    'click .remove': function (e, value, row, index) {
        if (confirm("is delete?")) {
            $.ajax({
                type: "post",
                url: '/admin/EmailAccount/delete/' + value + '',
                success: function (data) {
                    if (data["code"] == 1) {
                        alert(data['message']);
                        $table.bootstrapTable('remove', { field: 'id', values: [row.id] });
                    } else {
                        alert(data['message']);
                    }
                }
            });
        }
    }
};

//初始化表格
$(function () {
    var myTable = new TableInit();
    myTable.Init();
});

//查询
$("#search_account").click(function () {
    $table.bootstrapTable('refreshOptions', { pageNumber: 1 });
});