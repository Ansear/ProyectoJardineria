// Obtén una referencia al formulario
const registrationForm = document.getElementById("registrationForm");

// Agrega un evento de escucha para el envío del formulario
registrationForm.addEventListener("submit", function (event) {
  // Evita que el formulario se envíe de forma predeterminada
  event.preventDefault();

  // Obtén los valores de los campos del formulario
  const name = document.querySelector(
    '[placeholder="Enter your full name"]'
  ).value;
  const email = document.querySelector(
    '[placeholder="Enter your email"]'
  ).value;
  const password = document.querySelector(
    '[placeholder="Enter your password"]'
  ).value;

  // Crea un objeto con los datos del usuario
  const userData = {
    name: name,
    email: email,
    password: password,
  };

  // Convierte el objeto a una cadena JSON
  const userDataJSON = JSON.stringify(userData);

  localStorage.setItem("userData", userDataJSON);
  console.log("Redirección a index.html");
  window.location.href = "index.html";
  console.log("Submit button clicked");
});
