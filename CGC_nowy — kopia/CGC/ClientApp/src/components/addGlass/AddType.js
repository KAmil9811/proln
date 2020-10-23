import React, { Component } from "react";
import './AddType.css';


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
                alert("Dodano typ szkła do bazy danych")
                this.props.history.push('/glassatibutes')
            })
    }

    cancelAddType = (event) => {
        this.props.history.push('/controlpanel')
    }

    render() {
        return (
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
                        <button type="button" className="cancel_add_gla_t" onClick={this.cancelAddType}>Anuluj</button>
                        <button type="button" className="adding_glass_t" onClick={this.handleAddType}>Dodaj</button>
                        
                    </div>

                </form>
            </div>
        );
    }
}