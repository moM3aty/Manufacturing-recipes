AOS.init();

// Function to adjust navbar responsively
function adjustNavbar() {
  const navbarCollapse = document.querySelector(".navbar-collapse");
  const logoImg = document.querySelector(".logo-img");
  const logoMain = document.querySelector(".logo-main");
  const logoSubtitle = document.querySelector(".logo-subtitle");
  const logoContainer = document.querySelector(".logo-container");
  const logoText = document.querySelector(".logo-text");
  const textEnd = document.querySelector(".text-end");

  // Ensure logo container and text are always in a row
  if (logoContainer) {
    logoContainer.style.flexDirection = "row";
    logoContainer.style.alignItems = "center";
  }

  if (logoText) {
    logoText.style.flexDirection = "row";
    logoText.style.alignItems = "center";
  }

  if (textEnd) {
    textEnd.style.textAlign = "right";
  }

  if (window.innerWidth < 576) {
    // Mobile view
    logoImg.style.width = "40px";
    logoImg.style.height = "40px";
    logoMain.style.fontSize = "1.1rem";
    logoMain.style.whiteSpace = "nowrap";
    logoSubtitle.style.fontSize = "0.7rem";
    logoSubtitle.style.whiteSpace = "nowrap";

    if (navbarCollapse) {
      navbarCollapse.classList.add("navbar-mobile");
    }

    // Add animation to logo
    logoImg.style.animation = "pulse 2s infinite";
  } else if (window.innerWidth < 768) {
    // Tablet view
    logoImg.style.width = "45px";
    logoImg.style.height = "45px";
    logoMain.style.fontSize = "1.2rem";
    logoMain.style.whiteSpace = "nowrap";
    logoSubtitle.style.fontSize = "0.75rem";
    logoSubtitle.style.whiteSpace = "nowrap";

    if (navbarCollapse) {
      navbarCollapse.classList.remove("navbar-mobile");
    }

    // Add animation to logo
    logoImg.style.animation = "pulse 2s infinite";
  } else if (window.innerWidth < 992) {
    // Small desktop view
    logoImg.style.width = "50px";
    logoImg.style.height = "50px";
    logoMain.style.fontSize = "1.3rem";
    logoMain.style.whiteSpace = "nowrap";
    logoSubtitle.style.fontSize = "0.8rem";
    logoSubtitle.style.whiteSpace = "nowrap";

    if (navbarCollapse) {
      navbarCollapse.classList.remove("navbar-mobile");
    }

    // Add animation to logo
    logoImg.style.animation = "pulse 2s infinite";
  } else {
    // Desktop view
    logoImg.style.width = "60px";
    logoImg.style.height = "60px";
    logoMain.style.fontSize = "1.5rem";
    logoMain.style.whiteSpace = "nowrap";
    logoSubtitle.style.fontSize = "1rem";
    logoSubtitle.style.whiteSpace = "nowrap";

    if (navbarCollapse) {
      navbarCollapse.classList.remove("navbar-mobile");
    }

    // Add animation to logo
    logoImg.style.animation = "pulse 2s infinite";
  }
}

// About Us section animations
function initAboutUsAnimations() {
  const aboutSection = document.getElementById("about-us");
  if (!aboutSection) return;

  const aboutTitle = aboutSection.querySelector("h2");
  const aboutTexts = aboutSection.querySelectorAll(".animate-text");
  const aboutImage = aboutSection.querySelector("img");

  if (aboutImage) {
    aboutImage.classList.add("floating-image");
  }

  // Add animation classes when section is visible
  if (aboutTitle) {
    setTimeout(() => {
      aboutTitle.classList.add("animate-border");
    }, 500);
  }

  // Animate paragraphs with delay
  if (aboutTexts.length > 0) {
    aboutTexts.forEach((text, index) => {
      setTimeout(() => {
        text.style.opacity = "1";
        text.style.transform = "translateY(0)";
      }, 800 + 300 * index);
    });
  }
}

// Language switch functionality
const translations = {
  ar: {
    "nav-home": "الرئيسية",
    "nav-agriculture": "الزراعية",
    "nav-chemical": "الكيميائية",
    "nav-contact": "اتصل بنا",
    // Add more translations as needed
  },
  en: {
    "nav-home": "Home",
    "nav-agriculture": "Agriculture",
    "nav-chemical": "Chemical",
    "nav-contact": "Contact",
    // Add more translations as needed
  },
};

function setLang(lang) {
  document.documentElement.lang = lang === "ar" ? "ar" : "en";
  document.documentElement.dir = lang === "ar" ? "rtl" : "ltr";
  for (const key in translations[lang]) {
    const el = document.querySelector(`[data-i18n="${key}"]`);
    if (el) el.textContent = translations[lang][key];
  }
  document.getElementById("langSwitch").textContent =
    lang === "ar" ? "EN" : "AR";
  localStorage.setItem("lang", lang);
}

// Add event listener to language switch button
if (document.getElementById("langSwitch")) {
  document.getElementById("langSwitch").addEventListener("click", function () {
    const current = localStorage.getItem("lang") || "ar";
    setLang(current === "ar" ? "en" : "ar");
  });
}

// Set language on page load
window.addEventListener("DOMContentLoaded", function () {
  setLang(localStorage.getItem("lang") || "ar");
});

// Call adjustNavbar on page load and window resize
window.addEventListener("load", adjustNavbar);
window.addEventListener("resize", adjustNavbar);

document.addEventListener("DOMContentLoaded", function () {
  // Show More Button Functionality
  const showMoreBtn = document.getElementById("showMoreBtn");
  if (showMoreBtn) {
    showMoreBtn.addEventListener("click", function () {
      const extraCards = document.querySelectorAll(".extra-card");
      extraCards.forEach((card) => {
        card.classList.toggle("d-none");
      });
      this.textContent = this.textContent.includes("إظهار")
        ? "عرض أقل"
        : "إظهار المزيد";
    });
  }

  // Modal Functionality
  (function () {
    // Remove any previous event listeners to avoid duplicates
    const offerLinks = document.querySelectorAll(".card .btn-primary");
    const offerModal = document.getElementById("offerModal");
    let modalInstance = null;

    offerLinks.forEach((link) => {
      link.addEventListener("click", function (e) {
        e.preventDefault();
        const card = this.closest(".card");
        const title = card.querySelector(".card-title").textContent;
        const image = card.querySelector(".card-img-top").src;
        const description = card.querySelector(
          ".offer-full-description"
        ).textContent;

        document.getElementById("offerModalLabel").textContent = title;
        document.getElementById("offerImage").src = image;
        document.getElementById("offerDescription").textContent = description;

        // Ensure z-index is high enough
        offerModal.style.zIndex = 20000;
        // Show modal
        if (!modalInstance) {
          modalInstance = new bootstrap.Modal(offerModal, {
            backdrop: true,
            keyboard: true,
          });
        }
        modalInstance.show();
      });
    });

    // Ensure modal closes and resets instance
    offerModal.addEventListener("hidden.bs.modal", function () {
      if (modalInstance) {
        modalInstance.hide();
        modalInstance = null;
      }
    });

    // Ensure the action button works
    const actionBtn = offerModal.querySelector(".btn.btn-primary");
    if (actionBtn) {
      actionBtn.onclick = function () {
        alert("تم الاستفادة من العرض!\nOffer used!");
      };
    }
  })();
});
