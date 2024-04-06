const inputsRadio = document.querySelectorAll('input[type="radio"]');
console.log(inputsRadio);
inputsRadio.forEach((inputRadio) => {
    inputRadio.addEventListener('change', (event) => {
        console.log(event.target.value);
        const urlConQuery = window.location.href;
        const urlSplit = urlConQuery.split('?');
        const urlSinQuery = urlSplit[0];
        const urlQuery = urlSplit[1];
        const urlQueryParams = urlQuery.split('&');
        const paramsDiccioanrio = {};
        // Recorrer cada parámetro del query string y agregarlo al diccionario
        urlQueryParams.forEach(param => {
            const [key, value] = param.split('=');
            paramsDiccioanrio[key] = value;
        });

        // Mostrar el diccionario en la consola
        console.log('Diccionario de Parámetros:');
        console.log(paramsDiccioanrio);

        console.log('URL actual C query:', urlSplit[1]);
        console.log('URL actual C query:', urlConQuery);
        console.log('URL actual S query:', urlSinQuery);
        console.log('URL actual origin:', window.location.origin);

        // Mostrar los parámetros del query string en la consola
        console.log('Parámetros del Query String:');
        urlQueryParams.forEach(param => {
            console.log(param);
        });
    })


})