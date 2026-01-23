import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ThemeProvider } from "./context/ThemeContext";
import Layout from "./layout/Layout";
import AppRoutes from "./AppRoutes.jsx";

function App() {
    return (
        <ThemeProvider>
            <Router>
                <Layout>
                    <Routes>
                        {AppRoutes.map((route) => {
                            const { element, path } = route;
                            const key = path || "index";
                            return (
                                <Route key={key} {...route} element={element} />
                            );
                        })}
                    </Routes>
                </Layout>
            </Router>
        </ThemeProvider>
    );
}

export default App;
