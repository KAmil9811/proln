import React, { Component } from 'react';
import { OrdersTable } from './OrdersTable';



export class ReadyPackages extends Component {
    displayName = ReadyPackages.name;
    constructor(props) {
        super(props);
    }

    logOut = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('email')
        sessionStorage.removeItem('login')
        sessionStorage.removeItem('name')
        sessionStorage.removeItem('password')
        sessionStorage.removeItem('surname')
        sessionStorage.removeItem('user')
        sessionStorage.removeItem('admin')
        sessionStorage.removeItem('superAdmin')
        sessionStorage.removeItem('manager')
        sessionStorage.removeItem('magazineManagement')
        sessionStorage.removeItem('machineManagement')
        sessionStorage.removeItem('orderManagement')
        sessionStorage.removeItem('cutManagement')
        this.props.history.push('/')
    }


    homePage = (event) => {
        this.props.history.push('/home')
    }

    choosePackage = (event) => {
        this.props.history.push('/ready_packages')
    }

    render() {
        return (
            <div className="OrderWarehouse">
                <div className="nav_ow">
                    <button type="button" className="log_out" onClick={this.logOut}>Wyloguj</button>
                    <button type="button" className="home" onClick={this.homePage}>Strona główna</button>
                </div>
                <div className="conteiner_ow">
                    <div className="key">
                        <h3>LEGENDA</h3>
                        <p>X- ilość sztuk, na które brakuje materiału</p>
                        <p>Y- ilość oczekujących</p>
                        <p>Z- ilość w trakcie</p>
                    </div>
                    
                    <div className="tablewar">
                        <OrdersTable />
                    </div>
                </div>


            </div>
        );
    }
}