import { BrowserRouter, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import DashboardAdmin from "./pages/DashboardAdmin";
import DashboardUser from "./pages/DashboardUser";
import ProtectedRoute from "./components/ProtectedRoute";
import HomePage from "./pages/Home";
import CreateUser from "./pages/CreateUser";
import TemperatureProgramsPage from "./pages/TemperatureProgramsPage"; // adaugă acest import

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/admin"
          element={
            <ProtectedRoute role="Admin">
              <DashboardAdmin />
            </ProtectedRoute>
          }
        />
        <Route
         path="/admin/create-user"
         element={
        <ProtectedRoute role="Admin">
        <CreateUser />
        </ProtectedRoute>
        }
        />
        <Route
          path="/user"
          element={
            <ProtectedRoute role="User">
              <DashboardUser />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/temperature-programs"
          element={
            <ProtectedRoute role="Admin">
              <TemperatureProgramsPage />
            </ProtectedRoute>
          }
        />
        <Route
  path="/user/temperature-programs"
  element={
    <ProtectedRoute role="User">
      <TemperatureProgramsPage />
    </ProtectedRoute>
  }
/>
      </Routes>


    </BrowserRouter>
  );
}

export default App;
