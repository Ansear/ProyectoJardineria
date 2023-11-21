import "jsvectormap/dist/css/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "../css/style.css";

import Alpine from "alpinejs";
import persist from "@alpinejs/persist";
import flatpickr from "flatpickr";
import chart01 from "./components/chart-01";
import chart02 from "./components/chart-02";
import chart03 from "./components/chart-03";
import chart04 from "./components/chart-04";
import map01 from "./components/map-01";

Alpine.plugin(persist);
window.Alpine = Alpine;
Alpine.start();

// Init flatpickr
flatpickr(".datepicker", {
  // ... (configuración de flatpickr)
});

// Document Loaded
document.addEventListener("DOMContentLoaded", () => {
  chart01();
  chart02();
  chart03();
  chart04();
  map01();

  // Verifica si hay un token JWT almacenado y actualiza la vista del perfil
  const jwtToken = sessionStorage.getItem("jwtToken");
  if (jwtToken) {
    updateProfileView();
  }
});

// Función para obtener la información del usuario del sessionStorage
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
  const userNameElement = document.getElementById("userName");
  const userRoleElement = document.getElementById("userRole");
  const userProfileContainer = document.getElementById("userProfileContainer");

  // Verifica si los elementos existen antes de actualizar su contenido
  if (userNameElement) {
    userNameElement.textContent = userInfo.userName;
  }

  if (userRoleElement) {
    // Puedes mostrar los roles de usuario de una manera específica si es necesario
    // En este ejemplo, simplemente muestra la cadena JSON
    userRoleElement.textContent = JSON.stringify(userInfo.userRoles);
  }

  // Muestra el contenedor del perfil después de actualizar la información
  if (userProfileContainer) {
    userProfileContainer.classList.remove("hidden");
  }
}