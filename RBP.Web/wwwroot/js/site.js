function validateForm(form) {
    var is_valid = true;

    form.find("input[regex]")
        .each(function (i) {
            var e = $(this);
            var re = new RegExp(e.attr("regex"));
            if (re.test(e.val()) == false) {
                e.addClass("is-invalid");
                is_valid = false;
            } else {
                e.removeClass("is-invalid");
            }
        });

    return is_valid;
}

$("form.need-validate")
    .on("change keyup submit", function () {
        return validateForm($(this));
    })
    .each(function (i) {
        var form = $(this);
        var originData = form.serialize();
        form.find(":input").on('change input', function () {
            var hasChanges = form.serialize() !== originData;
            $('.saved-message').toggle(!hasChanges);
            $('.not-saved-message').toggle(hasChanges);
            form.find('.save-button').prop('disabled', !hasChanges);
        });
    })

const emptyView_ILS = `
<div class="card border-danger">
    <span class="text-danger mx-auto">Ничего нет</span>
</div>
`;

const template_ILS = `
<div class="card d-inline-block mr-1 mb-1 p-0 col-12 col-md-auto" onclick="showItem_ILS('{formId}','{index}')">
    <div class="d-flex flex-row align-items-center" style="flex-wrap: nowrap; overflow: hidden;">
        <p class="m-0 ml-2 text-nowrap" style="overflow: hidden; text-overflow: ellipsis;">
            <span class="font-weight-bold">{key}:&nbsp</span>
            <span>{value}</span>
        </p>
        <button class="btn btn-danger ml-auto ml-md-2 align-self-stretch" type="button" onclick="deleteItem_ILS('{formId}','{index}')" {disabled} style='width: 38px;'>
            &cross;
        </button>
    </div>
</div>
`;

function formatTemplate(template, kwargs) {
    return template.replace(/\{([a-zA-Z0-9_]+)\}/g, function (match, key) {
        return typeof kwargs[key] != 'undefined' ? kwargs[key] : match;
    });
};

function getItemsList_ILS(formId) {
    var targetInputId = $("#" + formId + "-target-input").val();
    var targetInput = $("#" + targetInputId);
    return JSON.parse(targetInput.val());
}

function setItemsList_ILS(formId, list) {
    var targetInputId = $("#" + formId + "-target-input").val();
    var targetInput = $("#" + targetInputId);
    targetInput.val(JSON.stringify(list)).trigger("change");
}

function showItem_ILS(formId, index) {
    var list = getItemsList_ILS(formId);
    var form = $("#" + formId);
    var item = list[index];
    form.find(":input").each(function (i) {
        var input = $(this);
        var name = input.attr("name")
        if (name != undefined && item[name] != undefined) {
            input.val(item[name]);
        }
    })
}

function deleteItem_ILS(formId, index) {
    var list = getItemsList_ILS(formId);
    list.splice(index, 1);
    setItemsList_ILS(formId, list);
    visualise_ILS(formId);
}

function addItem_ILS(formId) {
    var form = $("#" + formId);
    if (validateForm(form) == false) {
        return;
    }

    var item = {};
    form.find(":input").each(function (i) {
        var input = $(this);
        var name = input.attr("name")
        if (name != undefined) {
            item[name] = input.val();
        }
    })

    var list = getItemsList_ILS(formId);
    list.push(item);
    setItemsList_ILS(formId, list);
    visualise_ILS(formId);
}

function visualise_ILS(formId) {
    var form = $("#" + formId);
    var targetInputId = $("#" + formId + "-target-input").val();
    var targetListId = formId + "-list";
    var targetInput = $("#" + targetInputId);
    var targetList = $("#" + targetListId);
    var list = JSON.parse(targetInput.val());

    if (list.length == 0) {
        targetList.html(emptyView_ILS);
        return;
    }

    var map = JSON.parse($("#" + formId + "-map").val());
    var html = jQuery.map(list, function (value, index) {
        var viewData = {
            "key": value[map["key"]],
            "value": value[map["value"]],
            "index": index,
            "formId": formId,
            "disabled": form.attr("disabled")
        };
        return formatTemplate(template_ILS, viewData);
    }).join("");

    targetList.html(html);
}

$("form.list-input")
    .on("change paste keyup", function () {
        validateForm($(this));
    })
    .each(function (i) {
        var id = $(this).attr("id");
        visualise_ILS(id);
    })


const chartConfigs = {
    "one-line": {
        type: "line",
        data: {
            datasets: [
                {
                    backgroundColor: "#e52314",
                    borderColor: "#e52314",
                    borderWidth: 2,
                    hoverBackgroundColor: "#ed7817",
                    hoverBorderColor: "#ed7817"
                }
            ]
        },
        options: {
            maintainAspectRatio: false,
            scales: {
                y: {
                    stacked: true,
                    grid: {
                        display: true,
                        color: "rgba(0,0,0,0.2)"
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            },
            plugins: {
                legend: {
                    display: false
                }
            }
        }


    },
    "doughnut": {
        type: "doughnut",
        data: {
            datasets: [
                {
                    data: []
                }
            ]
        },
        options: {
            maintainAspectRatio: false,
            scales: {
                y: {
                    stacked: true,
                    grid: {
                        display: false
                    },
                    ticks: {
                        display: false
                    },
                    border: {
                        display: false
                    }
                },
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        display: false
                    },
                    border: {
                        display: false
                    }
                }
            }
        }
    }
}

$("canvas.chart").each(function (i) {
    var e = $(this);
    var chartConfigName = e.attr("chart-config");
    var chartConfig = JSON.stringify(chartConfigs[chartConfigName]);
    chartConfig = JSON.parse(chartConfig);
    chartConfig["data"]["labels"] = JSON.parse(e.attr("chart-labels"));
    chartConfig["data"]["datasets"][0]["data"] = JSON.parse(e.attr("chart-data"));

    new Chart(e, chartConfig);
})

$('[data-toggle="tooltip"]').tooltip()
