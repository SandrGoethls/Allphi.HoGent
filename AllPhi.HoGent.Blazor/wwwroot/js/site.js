function initializeInputListener() {
    var inputField = document.getElementById('street');
    if (inputField) {
        inputField.addEventListener('input', function (e) {
            var inputVal = e.target.value;
            var options = document.querySelectorAll('#streets option');
            for (var i = 0; i < options.length; i++) {
                if (options[i].value === inputVal) {
                    DotNet.invokeMethodAsync('AllPhi.HoGent.Blazor.Components', 'UpdateAddressDetails', inputVal);
                    break;
                }
            }
        });
    } else {
        console.error('Street input field was not found!');
    }
}



