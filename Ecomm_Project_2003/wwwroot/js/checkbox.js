$(document).ready(function () {
    $('#summarybtn').click(function (e) {
        e.preventDefault();
        var checkId = $('input[type=checkbox]:checked');
        if (checkId.length === 0) {
            alert("Please Select A Product");
            return false;
        }
        var checkInput = [];
        checkId.each(function () {
            checkInput.push($(this).val());
        });
        console.log("CheckBoxInput:", checkInput.join(','));
        $.ajax({
            url: "/Customer/Cart/Summary",
            method: "GET",
            data: {
                checkBoxInput: checkInput.join(',')
            },
            success: function (response) {
                window.location.href = "/Customer/Cart/Summary?checkBoxInput=" + checkInput.join(',');
            }
        })
    })
})