import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./shared/Header/Header";
import Footer from "./shared/Footer/Footer";
import Home from "./features/Home/Home";

const App = () => {
  return (
    <Router>
      <div id="app-container">
        <Header />
        <main id="content-wrapper">
          <Routes>
            <Route path="/" element={<Home />} />
          </Routes>
        </main>
        <Footer />
      </div>
    </Router>
  );
};

export default App;
