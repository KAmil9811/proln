
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
        console.log(sessionStorage.getItem('token'))
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
    goback2 = (event) => {
        this.props.history.push('/home')
    }


        render() {

        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1 className="texth1">Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
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
                            <button  className="ele3 production" onClick={this.Production}></button>
                            <button className="ele2 magazine" onClick={this.glassWarehouse}></button>
                            <button className="ele2 products" onClick={this.readyGlassWarehouse}></button>

                            <button className="ele2 orders" onClick={this.orderWarehouse}></button>
                            <button className="ele2 machines" onClick={this.machineWarehouse}></button>
                            <button className="ele2 account" onClick={this.userPanel}></button>
                            <button className="ele2 projects" onClick={this.saveProject}></button>
                            <button className="ele4 control" onClick={this.controlPanel}></button>
                        </div>
                    </form>
                </div>
            );
        }
    }


}