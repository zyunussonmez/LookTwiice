document.addEventListener("DOMContentLoaded", () => {

    const lightbox =
        document.getElementById("galleryLightbox");

    const image =
        document.getElementById("galleryLightboxImage");

    const caption =
        document.getElementById("galleryLightboxCaption");

    const close =
        document.getElementById("galleryLightboxClose");

    const prev =
        document.getElementById("galleryLightboxPrev");

    const next =
        document.getElementById("galleryLightboxNext");

    const backdrop =
        lightbox?.querySelector(
            ".gallery-lightbox__backdrop"
        );

    const photoButtons =
        Array.from(
            document.querySelectorAll(
                ".gallery-photo-button"
            )
        );


    if (!lightbox || photoButtons.length === 0) {
        return;
    }


    let currentIndex = 0;
    let previousBodyOverflow = "";


    function updateLightbox() {

        const button =
            photoButtons[currentIndex];

        if (!button) return;

        const photo =
            button.dataset.photo;

        const title =
            button.dataset.title || "";

        image.src = photo;
        image.alt = title;

        caption.textContent = title;
    }


    function openLightbox(index) {

        currentIndex = index;

        updateLightbox();

        previousBodyOverflow =
            document.body.style.overflow;

        document.body.style.overflow = "hidden";

        lightbox.classList.add("is-open");

        lightbox.setAttribute(
            "aria-hidden",
            "false"
        );
    }


    function closeLightbox() {

        lightbox.classList.remove("is-open");

        lightbox.setAttribute(
            "aria-hidden",
            "true"
        );

        document.body.style.overflow =
            previousBodyOverflow;

        setTimeout(() => {

            if (!lightbox.classList.contains("is-open")) {

                image.src = "";
                caption.textContent = "";

            }

        }, 300);
    }


    function showNext() {

        currentIndex =
            (currentIndex + 1)
            % photoButtons.length;

        updateLightbox();
    }


    function showPrevious() {

        currentIndex =
            (currentIndex - 1 +
                photoButtons.length)
            % photoButtons.length;

        updateLightbox();
    }


    photoButtons.forEach((button, index) => {

        button.addEventListener("click", () => {

            openLightbox(index);

        });

    });


    close.addEventListener(
        "click",
        closeLightbox
    );


    next.addEventListener(
        "click",
        showNext
    );


    prev.addEventListener(
        "click",
        showPrevious
    );


    backdrop.addEventListener(
        "click",
        closeLightbox
    );


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