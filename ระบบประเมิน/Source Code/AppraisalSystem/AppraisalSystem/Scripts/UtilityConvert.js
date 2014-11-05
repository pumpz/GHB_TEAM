$(function () {
    $(".money").keyup(function () {
        $(this).priceFormat({
            prefix: '',
            limit: 8,
            centsLimit: 0
        });
    });
});

function clearPrefix(value) {
  return value.replace(",", "");
}

function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
//$(function () {
//    $(".money_limit").keyup(function () {
//        $(this).priceFormat({
//            prefix: '',
//            limit: 10,
//            centsLimit: 0
//        });
//    });
//});

//$(function () {
//    $(".cust_limit").keyup(function () {
//        $(this).priceFormat({
//            prefix: '',
//            limit: 7,
//            centsLimit: 0
//        });
//    });
//});