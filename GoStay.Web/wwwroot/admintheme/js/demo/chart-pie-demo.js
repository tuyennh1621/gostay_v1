// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';
var _backgroundColor = ['#dc3545', '#2cc88a', '#36b9cc', '#fd7e14', '#6610f2', '#1e73df', '#17a2b8', '#e83e8c','#e74a3b'];
// Pie Chart Example
var ctx = document.getElementById("myPieChart");
var _labels = [];
var _data = [];
_labels = ctx.dataset.key.split(',');
_data = ctx.dataset.value.split(',');

var myPieChart1 = new Chart(ctx, {
  type: 'doughnut',
  data: {
      labels: _labels,
    datasets: [{
        data: _data,
        backgroundColor: _backgroundColor,
      hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
      hoverBorderColor: "rgba(234, 236, 244, 1)",
    }],
  },
  options: {
    maintainAspectRatio: false,
    tooltips: {
      backgroundColor: "rgb(255,255,255)",
      bodyFontColor: "#858796",
      borderColor: '#dddfeb',
      borderWidth: 1,
      xPadding: 15,
      yPadding: 15,
      displayColors: false,
      caretPadding: 10,
    },
    legend: {
      display: false
    },
    cutoutPercentage: 80,
  },
});


// Pie Chart Example
var ctx_PhanKhuc = document.getElementById("myPieChartPhanKhuc");

_labels = ctx_PhanKhuc.dataset.key.split(',');
_data = ctx_PhanKhuc.dataset.value.split(',');
var myPieChart = new Chart(ctx_PhanKhuc, {
    type: 'doughnut',
    data: {
        labels: _labels,
        datasets: [{
            data: _data,
            backgroundColor: _backgroundColor,
            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
            hoverBorderColor: "rgba(234, 236, 244, 1)",
        }],
    },
    options: {
        maintainAspectRatio: false,
        tooltips: {
            backgroundColor: "rgb(255,255,255)",
            bodyFontColor: "#858796",
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            caretPadding: 10,
        },
        legend: {
            display: false
        },
        cutoutPercentage: 80,
    },
});

var ctx_TypeHotel = document.getElementById("myLoaiChart");

_labels = ctx_TypeHotel.dataset.key.split(',');
_data = ctx_TypeHotel.dataset.value.split(',');
var myPieChart = new Chart(ctx_TypeHotel, {
    type: 'doughnut',
    data: {
        labels: _labels,
        datasets: [{
            data: _data,
            backgroundColor: _backgroundColor,
            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
            hoverBorderColor: "rgba(234, 236, 244, 1)",
        }],
    },
    options: {
        maintainAspectRatio: false,
        tooltips: {
            backgroundColor: "rgb(255,255,255)",
            bodyFontColor: "#858796",
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            caretPadding: 10,
        },
        legend: {
            display: false
        },
        cutoutPercentage: 80,
    },
});
