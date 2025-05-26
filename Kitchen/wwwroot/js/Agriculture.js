AOS.init({
    duration: 800,
    easing: "ease-in-out",
    once: true,
    mirror: false,
});

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

// Add smooth scrolling
document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
    anchor.addEventListener("click", function (e) {
        e.preventDefault();
        document.querySelector(this.getAttribute("href")).scrollIntoView({
            behavior: "smooth",
        });
    });
});

// Add intersection observer for fade-in effects
const observerOptions = {
    root: null,
    threshold: 0.1,
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
        if (entry.isIntersecting) {
            entry.target.classList.add("fade-in-up");
            observer.unobserve(entry.target);
        }
    });
}, observerOptions);

document.querySelectorAll(".card").forEach((card) => {
    observer.observe(card);
});

document.getElementById("showMoreBtn").addEventListener("click", function () {
    const moreRecipes = document.querySelector(".more-recipes");
    const btn = this;

    if (
        moreRecipes.style.display === "none" ||
        moreRecipes.style.display === ""
    ) {
        moreRecipes.style.display = "flex";
        btn.textContent = "عرض وصفات أقل";
    } else {
        moreRecipes.style.display = "none";
        btn.textContent = "عرض جميع الوصفات";
    }
});

// Download Modal logic
const downloadModal = new bootstrap.Modal(
    document.getElementById("downloadModal")
);
document
    .querySelectorAll(".card-footer-buttons .btn-accent")
    .forEach((btn, idx) => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();
            // Example: recipe files named recipe1.zip, recipe2.zip, ...
            const fileName = `recipe${idx + 1}.zip`;
            const link = document.getElementById("downloadRecipeLink");
            link.href = `downloads/${fileName}`;
            link.setAttribute("download", fileName);
            downloadModal.show();
        });
    });

// Call adjustNavbar on page load and window resize
window.addEventListener("load", adjustNavbar);
window.addEventListener("resize", adjustNavbar);

document.addEventListener("DOMContentLoaded", function () {
    // Call adjustNavbar when DOM is loaded
    adjustNavbar();
    // الترجمات للغة العربية والإنجليزية
    const translations = {
        ar: {
            "logo-title": "العلماء المتشردون",
            "logo-subtitle": "للتنمية العلمية",
            "nav-home": "الرئيسية",
            "nav-agriculture": "الزراعية",
            "nav-chemical": "الكيميائية",
            "nav-contact": "اتصل بنا",
            "hero-title": "وصفات التصنيع الزراعي",
            "hero-desc":
                "تركيبات مميزة لحماية المحاصيل، تحسين التربة، وزيادة الإنتاجية الزراعية. مصممة علمياً لتحقيق أفضل النتائج.",
            "hero-btn1": "تصفح الوصفات",
            "hero-btn2": "المزيد من المعلومات",
            "category-organic": "حلول عضوية",
            "category-organic-desc": "وصفات صديقة للبيئة",
            "category-protection": "حماية المحاصيل",
            "category-protection-desc": "تركيبات المبيدات الحشرية والفطرية",
            "category-soil": "محسنات التربة",
            "category-soil-desc": "تركيبات لتحسين جودة التربة",
            "category-growth": "منشطات النمو",
            "category-growth-desc": "خلطات لتسريع نمو النبات",
            "featured-recipes": "وصفات زراعية مميزة",
            "recipe1-title": "تركيبة مبيد فطري",
            "recipe1-desc":
                "فعال ضد الأمراض الفطرية الشائعة مع الحفاظ على البيئة. آمن للزراعة العضوية.",
            "recipe1-detail1": "الفعالية: 95% ضد البياض الدقيقي",
            "recipe1-detail2": "طريقة التطبيق: رش ورقي كل 7-10 أيام",
            "recipe1-detail3": "مدة الصلاحية: 6 أشهر",
            "download-btn": "تحميل",
            "cta-title": "هل أنت مستعد لتحويل إنتاجك الزراعي؟",
            "cta-desc":
                "انضم إلى آلاف المزارعين والمهنيين الزراعيين الذين يثقون بتركيباتنا لتحقيق محاصيل أفضل وزيادة في الإنتاج.",
            "cta-btn1": "ابدأ الآن",
            "cta-btn2": "اتصل بنا",
            "footer-desc":
                "وصفات وتركيبات مميزة للتصنيع الزراعي، الصيدلاني، الكيميائي، والغذائي.",
        },
        en: {
            "logo-title": "Vagabond Scientists",
            "logo-subtitle": "For Scientific Development",
            "nav-home": "Home",
            "nav-agriculture": "Agriculture",
            "nav-chemical": "Chemical",
            "nav-contact": "Contact Us",
            "hero-title": "Agricultural Manufacturing Recipes",
            "hero-desc":
                "Special formulas for crop protection, soil improvement, and increased agricultural productivity. Scientifically designed for best results.",
            "hero-btn1": "Browse Recipes",
            "hero-btn2": "More Information",
            "category-organic": "Organic Solutions",
            "category-organic-desc": "Eco-friendly formulas",
            "category-protection": "Crop Protection",
            "category-protection-desc": "Pesticide and fungicide formulations",
            "category-soil": "Soil Improvers",
            "category-soil-desc": "Formulas to improve soil quality",
            "category-growth": "Growth Stimulants",
            "category-growth-desc": "Mixtures to accelerate plant growth",
            "featured-recipes": "Featured Agricultural Recipes",
            "recipe1-title": "Fungicide Formulation",
            "recipe1-desc":
                "Effective against common fungal diseases while preserving the environment. Safe for organic farming.",
            "recipe1-detail1": "Effectiveness: 95% against powdery mildew",
            "recipe1-detail2": "Application: Foliar spray every 7-10 days",
            "recipe1-detail3": "Shelf life: 6 months",
            "download-btn": "Download",
            "cta-title": "Ready to transform your agricultural production?",
            "cta-desc":
                "Join thousands of farmers and agricultural professionals who trust our formulations for better crops and increased production.",
            "cta-btn1": "Get Started",
            "cta-btn2": "Contact Us",
            "footer-desc":
                "Special recipes and formulations for agricultural, pharmaceutical, chemical, and food manufacturing.",
        },
    };

    // وظيفة تغيير اللغة
    function setLanguage(lang) {
        // تغيير اتجاه الصفحة
        document.documentElement.lang = lang;
        document.documentElement.dir = lang === "ar" ? "rtl" : "ltr";

        // تغيير نص الزر
        document.getElementById("langSwitch").textContent =
            lang === "ar" ? "EN" : "AR";

        // تغيير رابط Bootstrap CSS بناءً على اللغة
        const bootstrapLink = document.querySelector('link[href*="bootstrap"]');
        if (lang === "ar") {
            bootstrapLink.href =
                "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.rtl.min.css";
        } else {
            bootstrapLink.href =
                "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css";
        }

        // تحديث جميع النصوص المترجمة
        const elements = document.querySelectorAll("[data-i18n]");
        elements.forEach((element) => {
            const key = element.getAttribute("data-i18n");
            if (translations[lang][key]) {
                element.textContent = translations[lang][key];
            }
        });

        // حفظ اللغة المفضلة
        localStorage.setItem("preferredLanguage", lang);
    }

    // تحميل اللغة المفضلة من localStorage
    const preferredLanguage = localStorage.getItem("preferredLanguage") || "ar";
    setLanguage(preferredLanguage);

    // حدث النقر على زر تغيير اللغة
    document.getElementById("langSwitch").addEventListener("click", function () {
        const currentLang = document.documentElement.lang;
        const newLang = currentLang === "ar" ? "en" : "ar";
        setLanguage(newLang);
    });

    // وظائف أخرى للصفحة (إن وجدت)
    document
        .getElementById("showMoreBtn")
        ?.addEventListener("click", function () {
            const moreRecipes = document.querySelector(".more-recipes");
            if (
                moreRecipes.style.display === "none" ||
                moreRecipes.style.display === ""
            ) {
                moreRecipes.style.display = "flex";
                this.textContent =
                    document.documentElement.lang === "ar"
                        ? "عرض وصفات أقل"
                        : "Show Less Recipes";
            } else {
                moreRecipes.style.display = "none";
                this.textContent =
                    document.documentElement.lang === "ar"
                        ? "عرض جميع الوصفات"
                        : "Show All Recipes";
            }
        });
});
