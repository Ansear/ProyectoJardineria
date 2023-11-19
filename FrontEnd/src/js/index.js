import "jsvectormap/dist/css/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "../css/style.css";

import Alpine from "alpinejs";
import persist from '@alpinejs/persist'
import flatpickr from "flatpickr";
import chart01 from "./components/chart-01";
import chart02 from "./components/chart-02";
import chart03 from "./components/chart-03";
import chart04 from "./components/chart-04";
import map01 from "./components/map-01";

Alpine.plugin(persist)
window.Alpine = Alpine;
Alpine.start();

// Init flatpickr
flatpickr(".datepicker", {
  mode: "range",
  static: true,
  monthSelectorType: "static",
  dateFormat: "M j, Y",
  defaultDate: [new Date().setDate(new Date().getDate() - 6), new Date()],
  prevArrow:
    '<svg class="fill-current" width="7" height="11" viewBox="0 0 7 11"><path d="M5.4 10.8l1.4-1.4-4-4 4-4L5.4 0 0 5.4z" /></svg>',
  nextArrow:
    '<svg class="fill-current" width="7" height="11" viewBox="0 0 7 11"><path d="M1.4 10.8L0 9.4l4-4-4-4L1.4 0l5.4 5.4z" /></svg>',
  onReady: (selectedDates, dateStr, instance) => {
    // eslint-disable-next-line no-param-reassign
    instance.element.value = dateStr.replace("to", "-");
    const customClass = instance.element.getAttribute("data-class");
    instance.calendarContainer.classList.add(customClass);
  },
  onChange: (selectedDates, dateStr, instance) => {
    // eslint-disable-next-line no-param-reassign
    instance.element.value = dateStr.replace("to", "-");
  },
});

// Document Loaded
document.addEventListener("DOMContentLoaded", () => {
  chart01();
  chart02();
  chart03();
  chart04();
  map01();
});

// Tu función asíncrona para realizar la solicitud GET
let enviar = async () => {
  try {
    const peticion = await fetch("http://localhost:5286/api/country");
    let res = await peticion.json();

    // Limpia el contenido actual del div
    document.getElementById("resultado").innerHTML = "";

    // Itera sobre los elementos y agrégales al div
    res.map(element => {
      // Crea un nuevo elemento de párrafo
      const nuevoParrafo = document.createElement("p");
      
      // Establece el contenido del párrafo con el valor del id
      nuevoParrafo.textContent = element.name;

      // Agrega el párrafo al div
      document.getElementById("resultado").appendChild(nuevoParrafo);
    });
  } catch (error) {
    console.error("Error al obtener y mostrar los datos:", error);
  }
}

// Llama a la función para realizar la solicitud y mostrar los datos
enviar();

document.getElementById('signinbutton1').addEventListener('click', function() {
  // Redirige a la página index.html
  window.location.href = 'index.html';
});