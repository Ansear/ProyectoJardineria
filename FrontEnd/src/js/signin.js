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

// Document Loaded
document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.getElementById("loginForm");

  loginForm.addEventListener("submit", function (event) {
    event.preventDefault();

    const email = document.getElementById("emailInput").value;
    const password = document.getElementById("passwordInput").value;

    const userData = {
      email: email,
      password: password,
    };

    // Realiza una solicitud al servidor para autenticar al usuario
    fetch("http://localhost:5286/api/User/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userData),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(
            `Error de autenticación. Estado: ${response.status}, Mensaje: ${response.statusText}`
          );
        }
        return response.json();
      })
      .then((data) => {
        // Verifica si el usuario está autenticado
        if (data.isAuthenticated) {
          // Almacena el token JWT y otros detalles en el sessionStorage
          sessionStorage.setItem("jwtToken", data.token);
          sessionStorage.setItem("userName", data.userName);
          sessionStorage.setItem("userRoles", JSON.stringify(data.roles));

          // Redirige a la página de inicio después de iniciar sesión
          window.location.href = "index.html";
        } else {
          console.error("Error de autenticación:", data.message);
        }
      })
      .catch((error) => {
        console.error("Error de autenticación:", error.message);
      });
  });

  // Verifica si hay un token JWT almacenado y redirige a la página de inicio si es así
  const jwtToken = sessionStorage.getItem("jwtToken");
  if (jwtToken) {
    window.location.href = "index.html";
  }
});
