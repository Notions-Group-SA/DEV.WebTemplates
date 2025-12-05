let touchStartX = 0;
let touchEndX = 0;
const minSwipeDistance = 50;

window.initTouchCarousel = function (elementId, dotNetHelper) {
    const element = document.getElementById(elementId);

    if (!element) {
        console.error('Element not found:', elementId);
        return;
    }

    element.addEventListener('touchstart', function (e) {
        touchStartX = e.changedTouches[0].screenX;
    }, false);

    element.addEventListener('touchend', function (e) {
        touchEndX = e.changedTouches[0].screenX;
        handleGesture(dotNetHelper);
    }, false);

    // También agregamos soporte para mouse para pruebas en desktop
    let isMouseDown = false;

    element.addEventListener('mousedown', function (e) {
        isMouseDown = true;
        touchStartX = e.screenX;
    }, false);

    element.addEventListener('mouseup', function (e) {
        if (isMouseDown) {
            touchEndX = e.screenX;
            handleGesture(dotNetHelper);
            isMouseDown = false;
        }
    }, false);
};

function handleGesture(dotNetHelper) {
    const distance = touchEndX - touchStartX;

    if (distance < -minSwipeDistance) {
        // Swipe left (siguiente)
        dotNetHelper.invokeMethodAsync('HandleSwipe', 'left');
    }

    if (distance > minSwipeDistance) {
        // Swipe right (anterior)
        dotNetHelper.invokeMethodAsync('HandleSwipe', 'right');
    }
}