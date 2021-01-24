import Sidebar from '../Sidebar';
import React, { Component } from 'react';
import { OptiWarehouseTable } from './optiWarehouseTable';


export class OptiWarehouse extends Component {
    displayName = OptiWarehouse.name;
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

    
    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div>

                    <Sidebar />
                    <div className="nav_mw">
                        <button className="add_machine" onClick={this.addMachine}>Oblicz nową partię</button>
                        <OptiWarehouseTable />

                    </div>
                    <div className="conteiner_mw">

                    </div>



                </div >
            );
        }
    }
}
