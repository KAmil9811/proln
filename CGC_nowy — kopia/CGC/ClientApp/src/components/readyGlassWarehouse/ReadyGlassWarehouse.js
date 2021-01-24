import React, { Component } from 'react';
import { ReadyGlassTable } from './ReadyGlassTable';
import './ReadyGlassWarehouse.css'
import Sidebar from '../Sidebar';


export class ReadyGlassWarehouse extends Component {
    displayName = ReadyGlassWarehouse.name;
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

    glassWarehouse = (event) => {
        this.props.history.push('/glasswarehouse')
    }

    historyReadyGlass = (event) => {
        this.props.history.push('/ready_glass_history')
    }
  /*  history() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button type="button" className="add_glass" onClick={this.historyReadyGlass}>Historia gotowego produktu</button>
            )
        }
    }*/
    render() {
      //  let history = this.history();
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
                    <div className="conteiner_gw">

                    </div >

                    <div className="conteiner_rgw">

                        <div className="tablerdy">
                            < ReadyGlassTable />
                        </div>
                    </div>
                </div>
            );
        }
    }
}
