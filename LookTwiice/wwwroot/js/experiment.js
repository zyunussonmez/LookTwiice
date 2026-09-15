document.addEventListener("DOMContentLoaded", () => {

    if (typeof gsap === "undefined") {
        console.error("GSAP could not be loaded.");
        return;
    }


    const intro = document.querySelector(".experiment-intro");
    const line = document.querySelector(".photo-line");

    const photos = gsap.utils.toArray(".hanging-photo");

    const labels = gsap.utils.toArray(".photo-label");

    const reveal = document.querySelector(".experiment-reveal");
    const revealImage = document.querySelector(".reveal-image");
    const revealContent = document.querySelector(".reveal-content");


    /*
     * INITIAL STATE
     */

    gsap.set(photos, {
        opacity: 0
    });

    gsap.set(labels, {
        opacity: 0,
        y: -20
    });


    /*
     * MAIN TIMELINE
     */

    const tl = gsap.timeline({
        defaults: {
            ease: "power3.out"
        }
    });


    /*
     * 1 — INTRO
     */

    tl.fromTo(
        intro,
        {
            opacity: 0,
            y: 20
        },
        {
            opacity: 1,
            y: 0,
            duration: 1.2
        }
    );


    /*
     * 2 — INTRO FADES
     */

    tl.to(
        intro,
        {
            opacity: 0,
            scale: .96,
            duration: .8
        },
        "+=0.8"
    );


    /*
     * 3 — CLOTHESLINE
     */

    tl.to(
        line,
        {
            scaleX: 1,
            duration: 1.3,
            ease: "power2.inOut"
        }
    );


    /*
     * 4 — PHOTOS ARRIVE
     */

    photos.forEach((photo, index) => {

        const rotation = gsap.utils.random(-4, 4);

        tl.to(
            photo,
            {
                opacity: 1,
                y: 0,
                rotation: rotation,
                duration: .9,
                ease: "back.out(1.5)"
            },
            index === 0 ? "-=.4" : "-=.55"
        );


        /*
         * LABEL APPEARS
         */

        const label = photo.querySelector(".photo-label");

        if (label) {

            tl.to(
                label,
                {
                    opacity: 1,
                    y: 0,
                    duration: .45,
                    ease: "power2.out"
                },
                "-=.35"
            );

        }

    });


    /*
     * 5 — LITTLE PAUSE
     */

    tl.to({}, {
        duration: 1
    });


    /*
     * 6 — PHOTOS SWING
     */

    photos.forEach((photo, index) => {

        gsap.to(photo, {
            rotation: `+=${index % 2 === 0 ? 1.5 : -1.5}`,
            duration: 2.5 + index * .3,
            repeat: -1,
            yoyo: true,
            ease: "sine.inOut",
            delay: index * .15
        });

    });


    /*
     * 7 — WAIT FOR USER
     */

    tl.to({}, {
        duration: 1
    });


    /*
     * CLICK PHOTO → REVEAL
     */

    photos.forEach((photo, index) => {

        photo.style.cursor = "pointer";

        photo.addEventListener("click", () => {

            reveal.style.visibility = "visible";

            gsap.timeline()

                .fromTo(
                    reveal,
                    {
                        opacity: 0
                    },
                    {
                        opacity: 1,
                        duration: .8,
                        ease: "power2.inOut"
                    }
                )

                .fromTo(
                    revealImage,
                    {
                        scale: 1.15
                    },
                    {
                        scale: 1,
                        duration: 1.5,
                        ease: "power3.out"
                    },
                    "-=.6"
                )

                .to(
                    revealContent,
                    {
                        opacity: 1,
                        y: 0,
                        duration: .8
                    },
                    "-=.8"
                );

        });

    });

});