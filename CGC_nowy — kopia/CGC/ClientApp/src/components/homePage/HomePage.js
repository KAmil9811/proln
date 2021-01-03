
import React, { Component } from 'react';
import Sidebar from '../Sidebar';
import './HomePage.css'




export class HomePage extends Component {
    displayName = HomePage.name;
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

    controlPanel = (event) => {
        this.props.history.push('/controlpanel')
    }

    glassWarehouse = (event) => {
        this.props.history.push('/glasswarehouse')
    }

    machineWarehouse = (event) => {
        this.props.history.push('/machinewarehouse')
    }

    orderWarehouse = (event) => {
        this.props.history.push('/orderwarehouse')
    }
    readyGlassWarehouse = (event) => {
        this.props.history.push('/ready_glass_warehouse')
    }
    Production = (event) => {
        this.props.history.push('/selection_of_orders')
    }

    render() {
        return (
            <div className="HomePage">
                <form>
                    <Sidebar />
                    <div className="conteiner">
                        <button className="ele2" onClick={this.Production}>Produkcja</button>
                        <button className="ele2" onClick={this.glassWarehouse}>Magazyn</button>
                        <button className="ele2" onClick={this.readyGlassWarehouse}>Gotowe produkty</button>
                        
                        <button className="ele2" onClick={this.orderWarehouse}>Zlecenia</button>
                        <button className="ele2" onClick={this.machineWarehouse}>Maszyny</button>
                        <button className="ele2" onClick={this.controlPanel}>Panel sterowania</button>
                    </div>
                </form>
            </div>
        );
    }


}