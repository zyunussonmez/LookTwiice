document.addEventListener("DOMContentLoaded", () => {

    /* ==========================================
       CATEGORY SMOOTH SCROLL
       ========================================== */

    const categoryLinks =
        document.querySelectorAll(".category-photo");

    categoryLinks.forEach(link => {

        link.addEventListener("click", event => {

            const target =
                document.querySelector(link.getAttribute("href"));

            if (!target) return;

            event.preventDefault();

            target.scrollIntoView({
                behavior: "smooth",
                block: "start"
            });

        });

    });


    /* ==========================================
       PORTFOLIO LIGHTBOX
       ========================================== */

    const lightbox =
        document.getElementById("portfolioLightbox");

    const lightboxImage =
        document.getElementById("lightboxImage");

    const lightboxCaption =
        document.getElementById("lightboxCaption");

    const lightboxClose =
        document.getElementById("lightboxClose");

    const lightboxPrev =
        document.getElementById("lightboxPrev");

    const lightboxNext =
        document.getElementById("lightboxNext");

    const lightboxBackdrop =
        lightbox?.querySelector(".portfolio-lightbox__backdrop");

    const photoButtons =
        Array.from(
            document.querySelectorAll(".portfolio-photo__button")
        );


    if (!lightbox || photoButtons.length === 0) {
        return;
    }


   let currentIndex = 0;
   let currentCategory = null;
   let currentCategoryPhotos = [];

   let previousBodyOverflow = "";

    /* ==========================================
       OPEN
       ========================================== */

    function openLightbox(button) {

    currentCategory =
        button.dataset.category;

    currentCategoryPhotos =
        photoButtons.filter(photoButton =>
            photoButton.dataset.category === currentCategory
        );

    currentIndex =
        currentCategoryPhotos.indexOf(button);

    updateLightbox();

    previousBodyOverflow =
        document.body.style.overflow;

    document.body.style.overflow = "hidden";

    lightbox.classList.add("is-open");

    lightbox.setAttribute("aria-hidden", "false");
}


    /* ==========================================
       UPDATE
       ========================================== */

    function updateLightbox() {

    const button =
        currentCategoryPhotos[currentIndex];

    if (!button) return;

    const imageUrl =
        button.dataset.photo;

    const title =
        button.dataset.title || "";

    lightboxImage.src = imageUrl;

    lightboxImage.alt = title;

    lightboxCaption.textContent = title;
}


    /* ==========================================
       CLOSE
       ========================================== */

    function closeLightbox() {

        lightbox.classList.remove("is-open");

        lightbox.setAttribute("aria-hidden", "true");

        document.body.style.overflow =
            previousBodyOverflow;

        /*
         * Görselin bir sonraki açılışta eski
         * görüntüsünü göstermemesi için temizliyoruz.
         */

        setTimeout(() => {

            if (!lightbox.classList.contains("is-open")) {

                lightboxImage.src = "";

                lightboxCaption.textContent = "";

            }

        }, 300);
    }


    /* ==========================================
       NEXT
       ========================================== */

    function showNext() {

    currentIndex =
        (currentIndex + 1) %
        currentCategoryPhotos.length;

    updateLightbox();
}


    /* ==========================================
       PREVIOUS
       ========================================== */

    function showPrevious() {

    currentIndex =
        (currentIndex - 1 +
            currentCategoryPhotos.length)
        % currentCategoryPhotos.length;

    updateLightbox();
}


    /* ==========================================
       PHOTO CLICK
       ========================================== */

    photoButtons.forEach(button => {

    button.addEventListener("click", () => {

        openLightbox(button);

    });

});

    /* ==========================================
       BUTTON EVENTS
       ========================================== */

    lightboxClose.addEventListener(
        "click",
        closeLightbox
    );

    lightboxNext.addEventListener(
        "click",
        showNext
    );

    lightboxPrev.addEventListener(
        "click",
        showPrevious
    );


    /* ==========================================
       BACKDROP CLICK
       ========================================== */

    lightboxBackdrop.addEventListener(
        "click",
        closeLightbox
    );


    /* ==========================================
       KEYBOARD
       ========================================== */

    document.addEventListener("keydown", event => {

        if (!lightbox.classList.contains("is-open")) {
            return;
        }

        switch (event.key) {

            case "Escape":
                closeLightbox();
                break;

            case "ArrowRight":
                showNext();
                break;

            case "ArrowLeft":
                showPrevious();
                break;
        }

    });

});