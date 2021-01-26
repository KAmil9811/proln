
import React, { Component } from 'react';
import Sidebar from '../Sidebar';
import './HomePage.css'




export class HomePage extends Component {
    displayName = HomePage.name;
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        var title = 'Home'
        sessionStorage.setItem('title', this.title)
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
        this.props.history.push('/controlpaneladmin')
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
        saveProject= (event) => {
            this.props.history.push('/saved_projects')
        }

    userPanel = (event) => {
        this.props.history.push('/userpanel')

    }
        goback = (event) => {
            this.props.history.push('/')
        }

        render() {

        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else {
            return (
                <div className="HomePage">
                    <form>
                        <Sidebar />
                        <div className="title">
                            <h1 className="titletext">Home page</h1>
                        </div>
                        <div className="conteiner">
                            <button className="ele3" onClick={this.Production}>Production</button>
                            <button className="ele2" onClick={this.glassWarehouse}>Magazine</button>
                            <button className="ele2" onClick={this.readyGlassWarehouse}>Products</button>

                            <button className="ele2" onClick={this.orderWarehouse}>Orders</button>
                            <button className="ele2" onClick={this.machineWarehouse}>Machines</button>
                            <button className="ele2" onClick={this.userPanel}>Your account</button>
                            <button className="ele2" onClick={this.saveProject}>Saved projects</button>
                            <button className="ele4" onClick={this.controlPanel}>Control panel</button>
                        </div>
                    </form>
                </div>
            );
        }
    }


}