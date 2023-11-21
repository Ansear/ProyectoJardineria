import "jsvectormap/dist/css/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "../css/style.css";

import Alpine from "alpinejs";
import persist from "@alpinejs/persist";
import flatpickr from "flatpickr";

Alpine.plugin(persist);
window.Alpine = Alpine;
Alpine.start();

// Tu función para hacer la solicitud a la API y manejar la respuesta
function fetchDataAndRenderTable(apiEndpoint) {
    fetch(apiEndpoint)
      .then(response => response.json())
      .then(data => renderTable(data))
      .catch(error => console.error('Error fetching data:', error));
  }
  
  // Función para renderizar la tabla dinámica
  function renderTable(data) {
    // Obtener el contenedor de la tabla
    const tableContainer = document.getElementById('dynamicTable');
  
    // Crear la tabla
    const table = document.createElement('div');
    table.className = 'flex flex-col';
  
    // Crear y agregar las columnas
    const headerRow = document.createElement('div');
    headerRow.className = `grid grid-cols-${Object.keys(data[0]).length} border-b border-stroke dark:border-strokedark bg-gray-2 dark:bg-meta-4`;
  
    for (const key in data[0]) {
      const column = document.createElement('div');
      column.className = 'p-2.5 text-center';
      const columnName = document.createElement('h5');
      columnName.className = 'text-sm font-medium uppercase';
      columnName.textContent = key;
      column.appendChild(columnName);
      headerRow.appendChild(column);
    }
  
    table.appendChild(headerRow);
  
    // Crear y agregar las filas
    data.forEach(item => {
      const row = document.createElement('div');
      row.className = `grid grid-cols-${Object.keys(data[0]).length} border-b border-stroke dark:border-strokedark`;
  
      for (const key in item) {
        const cell = document.createElement('div');
        cell.className = 'p-2.5 text-center';
        const cellValue = document.createElement('p');
        cellValue.className = 'font-medium';
        cellValue.textContent = item[key];
        cell.appendChild(cellValue);
        row.appendChild(cell);
      }
  
      table.appendChild(row);
    });
  
    // Agregar la tabla al contenedor
    tableContainer.appendChild(table);
  }
  
  // Llamar a la función para obtener y renderizar los datos
  const apiEndpoint = 'http://localhost:5286/api/customer/CustomersWithoutPayments';
  fetchDataAndRenderTable(apiEndpoint);