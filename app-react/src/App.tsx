import React, { useEffect } from 'react';
import logo from './logo.svg';
import './App.css';
import axios, { AxiosResponse } from 'axios';

function App() {

  const urlClientes = `${process.env.REACT_APP_API_URL}/clientes`;

  useEffect(() => {
    axios.get(urlClientes).then((respuesta: AxiosResponse<any>) => {
      console.log(respuesta.data);
    })
  }, [])

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
		<label>Link de la documentación Swagger del API: <a href="http://localhost:5080/swagger/index.html" target="_blank">Swagger API</a></label>
      </header>
    </div>
  );
}

export default App;
