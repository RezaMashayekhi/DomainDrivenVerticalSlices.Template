import React from "react";

function Footer() {
    const date = new Date();
    const isoDate = date.toISOString().slice(0, 10);
    const timeOptions = {
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        hour12: true,
    };
    const currentTime = date.toLocaleTimeString("en-US", timeOptions);

    return (
        <footer className="App-footer">
            Current time: {isoDate}, {currentTime}
        </footer>
    );
}

export default Footer;
