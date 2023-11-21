import "jsvectormap/dist/css/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "../css/style.css";

import Alpine from "alpinejs";
import persist from "@alpinejs/persist";
import flatpickr from "flatpickr";

Alpine.plugin(persist);
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

function getUserInfoFromSessionStorage() {
  const userName = sessionStorage.getItem("userName");
  const userRoles = JSON.parse(sessionStorage.getItem("userRoles"));

  return {
    userName: userName,
    userRoles: userRoles,
  };
}

// Función para actualizar la vista del perfil con la información del usuario
function updateProfileView() {
  const userInfo = getUserInfoFromSessionStorage();

  // Busca los elementos en el DOM donde deseas mostrar la información del usuario
  const nameElement = document.querySelector(".text-2xl");
  const roleElement = document.querySelector(".font-medium");

  // Verifica si los elementos existen antes de actualizar su contenido
  if (nameElement) {
    nameElement.textContent = userInfo.userName;
  }

  if (roleElement) {
    // Puedes mostrar los roles de usuario de una manera específica si es necesario
    // En este ejemplo, simplemente muestra la cadena JSON
    roleElement.textContent = JSON.stringify(userInfo.userRoles);
  }
}

// Llama a la función para actualizar la vista del perfil cuando el DOM esté cargado
document.addEventListener("DOMContentLoaded", updateProfileView);
