import React, { Component } from 'react';
import { ReadyGlassTable } from './ReadyGlassTable';
import './ReadyGlassWarehouse.css'


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

    render() {
        return (
            <div>
                <div className="nav_rgw">
                        <button type="button" className="log_out3" onClick={this.logOut}>Wyloguj</button>
                        <button type="button" className="magazine3" onClick={this.glassWarehouse}>Magazyn</button>
                        <button type="button" className="home3" onClick={this.homePage}>Strona główna</button>
                </div >
                <div className="conteiner_rgw">

                        <div className= "tablerdy">
                            <ReadyGlassTable />
                        </div>
                </div>
            </div>
        );
    }
}
