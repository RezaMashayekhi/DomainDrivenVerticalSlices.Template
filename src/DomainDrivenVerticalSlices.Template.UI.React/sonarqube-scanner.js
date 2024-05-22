const scanner = require("sonarqube-scanner");
scanner(
    {
        serverUrl: process.env.SONAR_HOST_URL,
        token: process.env.SONAR_TOKEN,
        options: {
            "sonar.projectKey": process.env.SONAR_PROJECT_KEY,
            "sonar.sourceEncoding": "UTF-8",
            "sonar.sources": "./src",
            "sonar.test.inclusions": "**/*.test.jsx,**/*.test.js",
            "sonar.exclusions":
                "**/index.jsx, **/reportWebVitals.js, **/*.test.js, **/*.test.jsx",
            "sonar.tests": "./src",
            "sonar.testExecutionReportPaths": "test-report.xml",
            "sonar.javascript.lcov.reportPaths": "coverage/lcov.info",
        },
    },
    () => process.exit()
);
