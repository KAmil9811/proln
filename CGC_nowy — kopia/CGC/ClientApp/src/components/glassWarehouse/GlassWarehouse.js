import React, { Component } from 'react';
import { GlassTable } from './GlassTable';
import './GlassWarehouse.css'

export class GlassWarehouse extends Component {
    displayName = GlassWarehouse.name;
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

    addGlass = (event) => {
        this.props.history.push('/add_glass')
    }

    historyGlass = (event) => {
        this.props.history.push('/glass_history')
    }

    delGlass = (event) => {
        var amount = prompt("Podaj Id szkła które chcesz usunąć:");
        var amount2 = parseInt(amount)
        sessionStorage.setItem('idg', amount)
        console.log(amount)
        if (amount === null) {
            return;
        }
        else if (isNaN(amount2)) {
            alert("Proszę wprowadzić liczbę!");
        }
        else {
            const receiver = {
                user: {
                    login: sessionStorage.getItem('login')
                },
                id_glass: sessionStorage.getItem('idg')
            }

            fetch(`api/Magazine/Remove_Glass`, {
                method: "post",
                body: JSON.stringify(receiver),
                headers: {
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then(json => {
                    console.log(json)
                    return (json);
                })
                .then(json => {
                    if (json[0].error_Messege === "Glass_Id_no_exist") {
                        alert("Szkło o podanym id nie istnieje")
                    }
                    else {
                        alert("Usunięto szkło o id:" + amount2)
                        window.location.reload();
                    }
                    
                })
            .then(json => {
            window.location.reload();
        })
        }

       
    }

    readyGlassWarehouse = (event) => {
        this.props.history.push('/ready_glass_warehouse')
    }

    render() {
        return (
            <div>
                <div className="nav_gw">
                    <button type="button" className="log_out2" onClick={this.logOut}>Wyloguj</button>
                      <button type="button" className=" redy_products" >Gotowe produkty</button>
                      
                </div>

                <div className="conteiner_gw">
                    <button type="button" className="del_glass_x" onClick={this.delGlass}>Usuń szkło</button>
                    <button type="button" className="add_glass" onClick={this.addGlass}>Dodaj szkło</button>
                    <button type="button" className="add_glass" onClick={this.historyGlass}>Historia szkła</button>

                    <div className="tableglass">
                        <GlassTable />
                    </div>
                </div>
            </div>
        );
    }
}
