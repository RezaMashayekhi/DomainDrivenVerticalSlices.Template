import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Footer from "./components/Footer/Footer";
import AppRoutes from "./AppRoutes";
import "./App.css";

function App() {
    return (
        <Router>
            <div className="App">
                <header className="App-header">
                    Domain Driven Vertical Slices
                </header>
                <main>
                    <Routes>
                    {AppRoutes.map((route) => {
                        const { element, path } = route;
                        const key = path || 'index';
                        return <Route key={key} {...route} element={element} />;
                    })}
                    </Routes>
                </main>
                <Footer />
            </div>
        </Router>
    );
}

export default App;
