import React, { Component } from "react";
import './AddType.css';
import Sidebar from '../Sidebar';


export class AddType extends Component {
    displayName = AddType.name;
    constructor(props) {
        super(props);
    }

    handleAddType = (event) => {
        event.preventDefault();
        const receiver = {
            type: this.refs.type.value,
            user: {
                login: sessionStorage.getItem('login')
            }
        }

        fetch(`api/Magazine/Add_type_Admin`, {
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
                if (json[0] === 'Type_alredy_exist') {
                    alert("Ten typ już istnieje")
                }
                else {
                    alert("Dodano typ szkła do bazy danych")
                    this.props.history.push('/glassatibutes')
                }
            })
    }

    cancelAddType = (event) => {
        this.props.history.push('/controlpaneladmin')
    }

    render() {
        return (
            <div className="Addtype">
                    <Sidebar />
                    <div className="addType">
                        <form>
                            <div className="form-group">
                                <h2>Dodaj typ szkła:</h2>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputType"
                                    placeholder="Wprowadź typ szkła"
                                    ref="type"
                                />
                            </div>
                            <div className="form-group">
                            <button type="button" className="danger_glass_type_add" onClick={this.cancelAddType}>Anuluj</button>
                            <button type="button" className="success_glass_type_add" onClick={this.handleAddType}>Dodaj</button>
                        
                            </div>

                        </form>
                </div>
            </div>
        );
    }
}