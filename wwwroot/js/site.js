document.addEventListener("click", (event) => {
  const toggle = event.target.closest("[data-toggle-password]");
  if (toggle) {
    const input = document.getElementById(toggle.dataset.togglePassword);
    if (input) {
      input.type = input.type === "password" ? "text" : "password";
      toggle.textContent = input.type === "password" ? "ver" : "ocultar";
    }
  }

  const keyCard = event.target.closest(".key-card");
  if (keyCard) {
    document.querySelectorAll(".key-card.selected").forEach((card) => {
      card.classList.remove("selected");
    });

    keyCard.classList.add("selected");

    const details = document.getElementById("keyDetails");
    const detailCode = document.getElementById("detailCode");
    const detailLocation = document.getElementById("detailLocation");

    if (details && detailCode && detailLocation) {
      detailCode.textContent = keyCard.dataset.code || "";
      detailLocation.textContent = keyCard.dataset.location || "";
      details.hidden = false;
    }
  }
});

document.addEventListener("input", (event) => {
  if (event.target.id !== "keySearch") {
    return;
  }

  const value = event.target.value.toLowerCase();
  document.querySelectorAll(".key-card").forEach((card) => {
    const text = `${card.dataset.code || ""} ${card.dataset.location || ""}`.toLowerCase();
    card.style.display = text.includes(value) ? "" : "none";
  });
});
