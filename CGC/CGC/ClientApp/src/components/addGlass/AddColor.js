import React, { Component } from "react";
import './AddColor.css';
import Sidebar from '../Sidebar';


export class AddColor extends Component {
    displayName = AddColor.name;
    constructor(props) {
        super(props);
    }

    handleAddColor = (event) => {
        event.preventDefault();
        const receiver = {
            color: this.refs.color.value,
            user: {
                login: sessionStorage.getItem('login')
            }
        }

        fetch(`api/Magazine/Add_Color_Admin`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })

            .then(res => res.json())
            .then(json => {
               
                return (json);
                
            })
            .then(json => {
                if (json[0] === 'Kolor juz istnieje') {
                    alert("Color already exist")
                }
                else {
                alert("You added new glass color")
                    this.props.history.push('/glassatibutes')
                }
            })
    }

    cancelAddColor = (event) => {
        this.props.history.push('/glassatibutes')
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
        else if (sessionStorage.getItem('machineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="AddColor">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Add glass color</h1>
                    </div>
                    <form>
                        <div className="AddColor_c">

                            <div className="form-group">

                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder="Enter glass color"
                                    ref="color"
                                />
                            </div>
                            <div className="form-group">
                                <button type="button" className="success_glass_color_add" onClick={this.handleAddColor}>Add color</button>

                                <button type="button" className="danger_glass_color_add" onClick={this.cancelAddColor}>Cancel</button>

                            </div>


                        </div>
                    </form>
                </div>

                );
        }
        else {
            return (
                <div className="HomePage">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login" onClick={this.goback2} >Back to home page</button>
                </div>
            );

        }
    }
}