
import './App.css';
import Login from './Login';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import ProtectedRouta from './ProtectedRouta';
import Prodavnica from './Prodavnica';
import ProtectedSecond from './ProtectedSecond';
import IzborPage from './IzborPage';
import Korpa from './Korpa';
function App() {
  return (
    <Router>
    <Routes>
      <Route element={<ProtectedRouta />}>
          <Route element={<Login/>} path="/" />
          
      </Route>
    <Route element={<ProtectedSecond/>}>
    <Route element={<IzborPage/>} path='/izbor'  />
     <Route element={<Prodavnica/>} path='/prodavnica'  />
     <Route element={<Korpa/>} path='/korpa'/>
    
     </Route>
    </Routes>
</Router>
  );
}

export default App;
