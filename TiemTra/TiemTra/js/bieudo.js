// ---------- CHARTS ----------

// BAR CHART
const barChartOptions = {
    series: [
        {
            data: [],
            name: 'Doanh thu',
        },
    ],
    chart: {
        type: 'bar',
        background: 'transparent',
        height: 350,
        toolbar: {
            show: false,
        },
    },
    colors: ['#2962ff'],
    plotOptions: {
        bar: {
            distributed: true,
            borderRadius: 4,
            horizontal: false,
            columnWidth: '40%',
        },
    },
    dataLabels: {
        enabled: false,
    },
    fill: {
        opacity: 1,
    },
    grid: {
        borderColor: '#55596e',
        yaxis: {
            lines: {
                show: true,
            },
        },
        xaxis: {
            lines: {
                show: true,
            },
        },
    },
    legend: {
        labels: {
            colors: '#f5f7ff',
        },
        show: true,
        position: 'top',
    },
    stroke: {
        colors: ['transparent'],
        show: true,
        width: 2,
    },
    tooltip: {
        shared: true,
        intersect: false,
        theme: 'dark',
    },
    xaxis: {
        categories: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
        title: {
            style: {
                color: '#f5f7ff',
            },
        },
        axisBorder: {
            show: true,
            color: '#55596e',
        },
        axisTicks: {
            show: true,
            color: '#55596e',
        },
        labels: {
            style: {
                colors: '#f5f7ff',
            },
        },
    },
    yaxis: {
        title: {
            text: 'Doanh thu',
            style: {
                color: '#f5f7ff',
            },
        },
        axisBorder: {
            color: '#55596e',
            show: true,
        },
        axisTicks: {
            color: '#55596e',
            show: true,
        },
        labels: {
            style: {
                colors: '#f5f7ff',
            },
        },
    },
};

const barChart = new ApexCharts(
    document.querySelector('#bar-chart'),
    barChartOptions
);
barChart.render();

// AREA CHART
const areaChartOptions = {
    series: [
        {
            name: 'Doanh thu',
            data: [],
        },
        {
            name: 'Chi Tieu',
            data: [],
        },
    ],
    chart: {
        type: 'area',
        background: 'transparent',
        height: 350,
        stacked: false,
        toolbar: {
            show: false,
        },
    },
    colors: ['#00ab57', '#d50000'],
    dataLabels: {
        enabled: false,
    },
    fill: {
        gradient: {
            opacityFrom: 0.4,
            opacityTo: 0.1,
            shadeIntensity: 1,
            stops: [0, 100],
            type: 'vertical',
        },
        type: 'gradient',
    },
    grid: {
        borderColor: '#55596e',
        yaxis: {
            lines: {
                show: true,
            },
        },
        xaxis: {
            lines: {
                show: true,
            },
        },
    },
    legend: {
        labels: {
            colors: '#f5f7ff',
        },
        show: true,
        position: 'top',
    },
    markers: {
        size: 6,
        strokeColors: '#1b2635',
        strokeWidth: 3,
    },
    stroke: {
        curve: 'smooth',
    },
    xaxis: {
        categories: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
        axisBorder: {
            color: '#55596e',
            show: true,
        },
        axisTicks: {
            color: '#55596e',
            show: true,
        },
        labels: {
            offsetY: 5,
            style: {
                colors: '#f5f7ff',
            },
        },
    },
    yaxis: [
        {
            title: {
                text: 'Doanh Thu',
                style: {
                    color: '#f5f7ff',
                },
            },
            labels: {
                style: {
                    colors: ['#f5f7ff'],
                },
            },
        },
        {
            opposite: true,
            title: {
                text: 'Chi Tiêu',
                style: {
                    color: '#f5f7ff',
                },
            },
            labels: {
                style: {
                    colors: ['#f5f7ff'],
                },
            },
        },
    ],
    tooltip: {
        shared: true,
        intersect: false,
        theme: 'dark',
    },
};

const areaChart = new ApexCharts(
    document.querySelector('#area-chart'),
    areaChartOptions
);
areaChart.render();



function updateChart(selectedYear) {
    $.ajax({
        url: '/Account/LoadBarChart',
        type: 'GET',
        data: { selectedYear: selectedYear },
        dataType: 'json',
        success: function (dataDoanhThu) {
            console.log('Received Doanh Thu data:', dataDoanhThu);

            // Process Doanh Thu data
            const doanhThuData = processData(dataDoanhThu);

            // Update 'Doanh Thu' data for both bar chart and area chart
            updateDoanhThuChart(doanhThuData);

            // Fetch 'Chi Tieu' data
            $.ajax({
                url: '/Account/ThuChi',
                type: 'GET',
                data: { selectedYear: selectedYear },
                dataType: 'json',
                success: function (dataChiTieu) {
                    console.log('Received Chi Tieu data:', dataChiTieu);

                    // Process Chi Tieu data
                    const chiTieuData = processData(dataChiTieu);

                    // Update 'Chi Tieu' data for the area chart
                    updateChiTieuChart(chiTieuData);
                },
                error: function (error) {
                    console.error('Error loading Chi Tieu data:', error);
                }
            });
        },
        error: function (error) {
            console.error('Error loading Doanh Thu data:', error);
        }
    });
}

// Sử dụng một mảng cố định của 12 tháng
const months = [
    'Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
    'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'
];

function processData(data) {
    // Tạo mảng mới với dữ liệu của 12 tháng, nếu không có dữ liệu, gán giá trị là 0
    const processedData = months.map((month, index) => {
        const entry = data.find(item => item.Month === (index + 1));
        return {
            Month: month,
            TotalRevenue: entry ? parseFloat(entry.TotalRevenue.toFixed(3)) : 0.000
        };
    });

    return processedData;
}

function updateDoanhThuChart(data) {
    // Tạo mảng dữ liệu cho biểu đồ Doanh Thu
    const doanhThuData = data.map(entry => entry.TotalRevenue);

    // Cập nhật 'Doanh Thu' cho cả bar chart và area chart
    barChartOptions.series = [{
        data: doanhThuData,
        name: 'Doanh thu',
    }];

    areaChartOptions.series = [{
        data: doanhThuData,
        name: 'Doanh thu',
    }];

    // Cập nhật biểu đồ
    barChart.updateOptions(barChartOptions);
    areaChart.updateOptions(areaChartOptions);
}

function updateChiTieuChart(data) {
    // Tạo mảng dữ liệu cho biểu đồ Chi Tieu
    const chiTieuData = data.map(entry => entry.TotalRevenue);

    // Cập nhật 'Chi Tieu' cho area chart
    areaChartOptions.series.push({
        data: chiTieuData,
        name: 'Chi Tieu',
    });

    // Cập nhật biểu đồ
    areaChart.updateOptions(areaChartOptions);
}




// Populate the years dropdown with actual values
$(document).ready(function () {
    // Get the current year
    const currentYear = new Date().getFullYear();

    // Set the default text for the current year
    const currentYearText = "Năm nay";

    // Assuming you have a variable 'availableYears' with the available years
    const availableYears = [currentYearText, 2024, 2023, 2022, 2021, 2020, 2019, 2018];

    // Populate the dropdown with years
    const yearDropdown = $('#selected-year');
    availableYears.forEach(year => {
        yearDropdown.append($('<option>', {
            value: year === currentYearText ? currentYear : year,
            text: year,
        }));
    });

    // Set the default selected year to "Năm nay"
    yearDropdown.val(currentYear);

    // Attach an event listener to update the chart when the year changes
    yearDropdown.change(function () {
        const selectedYear = $(this).val();
        updateChart(selectedYear === currentYearText ? currentYear : selectedYear);
    });

    // Trigger the initial chart update with the default selected year
    updateChart(currentYear);
}); 



