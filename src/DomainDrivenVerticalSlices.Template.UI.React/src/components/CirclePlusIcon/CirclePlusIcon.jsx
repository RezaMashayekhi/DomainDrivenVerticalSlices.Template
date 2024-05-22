import React from "react";
import "./CirclePlusIcon.css";

const CirclePlusIcon = () => (
    <svg
        className="circle-plus-icon"
        xmlns="http://www.w3.org/2000/svg"
        width="36"
        height="36"
        fill="currentColor"
        viewBox="0 0 16 16"
    >
        <circle cx="8" cy="8" r="8" fill="#74899f" />
        <path
            fill="#ffffff"
            d="M8 3.5a.5.5 0 0 1 .5.5v4h4a.5.5 0 0 1 0 1h-4v4a.5.5 0 0 1-1 0v-4H3.5a.5.5 0 0 1 0-1H7.5v-4a.5.5 0 0 1 .5-.5z"
        />
    </svg>
);

export default CirclePlusIcon;
