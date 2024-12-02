const EventBus = {
    on(event, callback) {
        window.addEventListener(event, callback);
    },
    emit(event, detail = {}) {
        window.dispatchEvent(new CustomEvent(event, { detail }));
    },
    off(event, callback) {
        window.removeEventListener(event, callback);
    }
};
