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

const registrationForm = document.getElementById("registrationForm");

registrationForm.addEventListener("submit", function (event) {
  event.preventDefault();

  const username = document.querySelector('[placeholder="Enter your full name"]').value;
  const email = document.querySelector('[placeholder="Enter your email"]').value;
  const password = document.querySelector('[placeholder="Enter your password"]').value;

  const userData = {
    username: username,
    email: email,
    password: password,
  };

  // Envía los datos del usuario al servidor para autenticación
  fetch("http://localhost:5286/api/User/register", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(userData),
  })
    .then((response) => {
      // Manejo especial para respuestas que no son JSON
      if (!response.ok) {
        return response.text().then((text) => {
          throw new Error(`Error en la solicitud: ${response.status}, Contenido: ${text}`);
        });
      }
      return response.text(); // Usamos response.text() ya que el servidor devuelve una cadena de texto
    })
    .then((data) => {
      // Aquí `data` contiene la cadena de texto devuelta por la API
      console.log("Respuesta de la API:", data);

      // Redirige a signin.html después de manejar la respuesta exitosa
      window.location.href = "signin.html";
    })
    .catch((error) => {
      console.error("Error de autenticación:", error);

      // Puedes mostrar un mensaje de error en tu formulario
      // o redirigir a una página de error, según tus necesidades
    });
});

document.addEventListener("DOMContentLoaded", function () {
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
      .then((response) => response.json())
      .then((data) => {
        // Almacena el token JWT en el localStorage
        localStorage.setItem("jwtToken", data.token);

        // Redirige a la página de inicio después de iniciar sesión
        window.location.href = "index.html";
      })
      .catch((error) => {
        console.error("Error de autenticación:", error);
      });
  });

  // Verifica si hay un token JWT almacenado y redirige a la página de inicio si es así
  const jwtToken = localStorage.getItem("jwtToken");
  if (jwtToken) {
    window.location.href = "index.html";
  }
});


// // Tu función asíncrona para realizar la solicitud GET
// let enviar = async () => {
//   try {
//     const peticion = await fetch("http://localhost:5286/api/country");
//     let res = await peticion.json();

//     // Limpia el contenido actual del div
//     document.getElementById("resultado").innerHTML = "";

//     // Itera sobre los elementos y agrégales al div
//     res.map((element) => {
//       // Crea un nuevo elemento de párrafo
//       const nuevoParrafo = document.createElement("p");

//       // Establece el contenido del párrafo con el valor del id
//       nuevoParrafo.textContent = element.name;

//       // Agrega el párrafo al div
//       document.getElementById("resultado").appendChild(nuevoParrafo);
//     });
//   } catch (error) {
//     console.error("Error al obtener y mostrar los datos:", error);
//   }
// };

// // Llama a la función para realizar la solicitud y mostrar los datos
// enviar();

// document.getElementById("signinbutton1").addEventListener("click", function () {
//   // Redirige a la página index.html
//   window.location.href = "./index.html";
// });
