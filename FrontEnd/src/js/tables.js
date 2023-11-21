import "jsvectormap/dist/css/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "../css/style.css";

import Alpine from "alpinejs";
import persist from "@alpinejs/persist";
import flatpickr from "flatpickr";

Alpine.plugin(persist);
window.Alpine = Alpine;
Alpine.start();

// Función para hacer la solicitud a la API y manejar la respuesta
function fetchDataAndRenderTable(apiEndpoint) {
  fetch(apiEndpoint)
    .then((response) => response.json())
    .then((data) => renderTable(data))
    .catch((error) => console.error("Error fetching data:", error));
}

// Función para renderizar la tabla dinámica
function renderTable(data) {
  // Obtener el contenedor de la tabla
  const tableContainer = document.getElementById("dynamicTable");
  tableContainer.innerHTML = ""; // Limpiar el contenedor antes de agregar la nueva tabla

  // Crear la tabla
  const table = document.createElement("div");
  table.className = "flex flex-col";

  // Crear y agregar las columnas
  const headerRow = document.createElement("div");
  headerRow.className = `grid grid-cols-${
    Object.keys(data[0]).length
  } border-b border-stroke dark:border-strokedark`;

  for (const key in data[0]) {
    const column = document.createElement("div");
    column.className = "p-2.5 text-center";
    const columnName = document.createElement("h5");
    columnName.className = "text-sm font-medium uppercase";
    columnName.textContent = key;
    column.appendChild(columnName);
    headerRow.appendChild(column);
  }

  table.appendChild(headerRow);

  // Crear y agregar las filas
  data.forEach((item) => {
    const row = document.createElement("div");
    row.className = `grid grid-cols-${
      Object.keys(data[0]).length
    } border-b border-stroke dark:border-strokedark`;

    for (const key in item) {
      const cell = document.createElement("div");
      cell.className = "p-2.5 text-center";
      const cellValue = document.createElement("p");
      cellValue.className = "font-medium";
      cellValue.textContent = item[key];
      cell.appendChild(cellValue);
      row.appendChild(cell);
    }

    table.appendChild(row);
  });

  // Agregar la tabla al contenedor
  tableContainer.appendChild(table);
}

// Manejar el cambio en la primera lista desplegable (categoría)
document
  .getElementById("categorySelector")
  .addEventListener("change", function () {
    const selectedCategory = this.value;

    // Obtener la segunda lista desplegable (consultas)
    const querySelector = document.getElementById("querySelector");
    querySelector.innerHTML = ""; // Limpiar la segunda lista desplegable

    // Obtener las consultas disponibles para la categoría seleccionada
    const queries = getQueriesForCategory(selectedCategory);

    // Llenar la segunda lista desplegable con las consultas disponibles
    queries.forEach((query) => {
      const option = document.createElement("option");
      option.value = query;
      option.textContent = query;
      querySelector.appendChild(option);
    });

    // Mostrar o ocultar el input según sea necesario
    const userInputContainer = document.getElementById("userInputContainer");
    userInputContainer.style.display = queries.some((query) =>
      query.includes("{")
    )
      ? "block"
      : "none";
  });

// Manejar el cambio en la segunda lista desplegable (consulta)
document
  .getElementById("querySelector")
  .addEventListener("change", function () {
    const selectedQuery = this.value;
    const selectedCategory = document.getElementById("categorySelector").value;

    // Mostrar u ocultar el input según sea necesario
    const userInputContainer = document.getElementById("userInputContainer");
    userInputContainer.style.display = selectedQuery.includes("{")
      ? "block"
      : "none";

    // Inicializar la tabla con los valores predeterminados
    const defaultApiEndpoint = `http://localhost:5286/api/${selectedCategory}/${selectedQuery}`;
    fetchDataAndRenderTable(defaultApiEndpoint);
  });

// Obtener las consultas disponibles para una categoría dada (simulación)
function getQueriesForCategory(category) {
  // Simulación: Deberías obtener esta información de tu API
  switch (category) {
    case "city":
      return [
        "CityAndPhoneFromOffices/{country}",
        "CustomerCountByCityStartingWithM",
        // Agrega más consultas según las disponibles para la categoría 'city'
      ];
    case "customer":
      return [
        "CustomerWithPaymentIn/{year}",
        "CustomersWithSalesRepresentatives",
        "CustomersWithoutPaymentsWithRepresentativesAndOffices",
        "ProductRangesByCustomer",
        "CustomersWithoutPayments",
        "GetCustomerWithEmployee",
        "GetCustomerEmployeeAndCity",
        "GetCustomersWithLateDelivering",
        "GetAllCountCustomer",
        "CustomersInMadridWithSalesRepresentatives",
        "CustomerCountByCountry",
        "CustomerWithHighestCreditLimit",
        "WithoutPayments",
        "WithPayments",
        "OrdersIn2008",
        // Agrega más consultas según las disponibles para la categoría 'customer'
      ];
    case "employee":
      return [
        "EmployeeNotSalesRepresentative",
        "EmployeesWithManagers",
        "EmployeesWithoutOffice",
        "EmployeesWithoutOfficeAndClients",
        "TotalEmployees",
        // Agrega más consultas según las disponibles para la categoría 'employee'
      ];
    case "office":
      return [
        "OfficeCity",
        "OfficesWithoutSalesRepresentativesForFruitProducts",
        // Agrega más consultas según las disponibles para la categoría 'office'
      ];
    case "order":
      return [
        "RejectedOrdersInYear/{year}",
        "OrdersCountByStatus",
        // Agrega más consultas según las disponibles para la categoría 'order'
      ];
    case "payment":
      return [
        "DistinctPaymentForms",
        // Agrega más consultas según las disponibles para la categoría 'payment'
      ];
    default:
      return [];
  }
}

// Inicializar la primera lista desplegable y la tabla con los valores predeterminados
const defaultCategory = "city";
const defaultQuery = "CityAndPhoneFromOffices/{country}";
document.getElementById("categorySelector").value = defaultCategory;
document.getElementById("querySelector").value = defaultQuery;
fetchDataAndRenderTable(
  `http://localhost:5286/api/${defaultCategory}/${defaultQuery}`
);
