
const woosmap_key = "woos-11a5c2a6-cbf1-3b4d-9ed0-29d40acef112";
const countryRestrictions = ["GB", "JE", "IM", "GG"];
const types = ["postal_code"];
let map;
let debouncedAutocomplete;
let inputElement;
let suggestionsList;
let clearSearchBtn;
let markerAddress;

function init() {
    if (inputElement && suggestionsList) {
        inputElement.addEventListener("input", handleAutocomplete);
        inputElement.addEventListener("keydown", (event) => {
            if (event.key === "Enter") {
                const firstLi = suggestionsList.querySelector("li");

                if (firstLi) {
                    firstLi.click();
                }
            }
        });
    }

    clearSearchBtn.addEventListener("click", () => {
        inputElement.value = "";
        suggestionsList.style.display = "none";
        clearSearchBtn.style.display = "none";
        inputElement.focus();
    });
    debouncedAutocomplete = debouncePromise(autocompleteAddress, 0);
}

function handleAutocomplete() {
    if (inputElement && suggestionsList) {
        const input = inputElement.value;

        input.replace('"', '\\"').replace(/^\s+|\s+$/g, "");
        if (input !== "") {
            debouncedAutocomplete(input)
                .then(({ localities }) => displaySuggestions(localities))
                .catch((error) =>
                    console.error("Error autocomplete localities:", error),
                );
        } else {
            suggestionsList.style.display = "none";
            clearSearchBtn.style.display = "none";
        }
    }
}

function displaySuggestions(localitiesPredictions) {
    if (inputElement && suggestionsList) {
        suggestionsList.innerHTML = "";
        if (localitiesPredictions.length > 0) {
            localitiesPredictions.forEach((locality) => {
                let _a;
                const li = document.createElement("li");

                li.innerHTML =
                    (_a = formatPredictionList(locality)) !== null && _a !== void 0
                        ? _a
                        : "";
                li.addEventListener("click", () => {
                    let _a;

                    inputElement.value =
                        (_a = locality.description) !== null && _a !== void 0 ? _a : "";
                    requestDetailsAddress(locality.public_id);
                    suggestionsList.style.display = "none";
                });
                suggestionsList.appendChild(li);
            });
            suggestionsList.style.display = "block";
            clearSearchBtn.style.display = "block";
        } else {
            suggestionsList.style.display = "none";
        }
    }
}

function formatPredictionList(locality) {
    const formattedName =
        locality.matched_substrings && locality.matched_substrings.description
            ? bold_matched_substring(
                locality.description,
                locality.matched_substrings.description,
            )
            : locality.description;
    const addresses = locality.has_addresses
        ? `<span class='light'>- view addresses</span>`
        : "";
    const predictionClass = locality.has_addresses
        ? `prediction-expandable`
        : "no-viewpoint";
    return `<div class="prediction ${predictionClass}">${formattedName} ${addresses}</div>`;
}

function bold_matched_substring(string, matched_substrings) {
    matched_substrings = matched_substrings.reverse();

    for (const substring of matched_substrings) {
        const char = string.substring(
            substring["offset"],
            substring["offset"] + substring["length"],
        );

        string = `${string.substring(0, substring["offset"])}<span class='bold'>${char}</span>${string.substring(substring["offset"] + substring["length"])}`;
    }
    return string;
}

function autocompleteAddress(input) {
    const args = {
        key: woosmap_key,
        input,
    };

    args["components"] = countryRestrictions
        .map((country) => `country:${country}`)
        .join("|");
    args["types"] = types.join("|");
    return fetch(
        `https://api.woosmap.com/localities/autocomplete/?${buildQueryString(args)}`,
    ).then((response) => response.json());
}

function buildQueryString(params) {
    const queryStringParts = [];

    for (const key in params) {
        if (params[key]) {
            const value = params[key];

            queryStringParts.push(
                `${encodeURIComponent(key)}=${encodeURIComponent(value)}`,
            );
        }
    }
    return queryStringParts.join("&");
}


function requestDetailsAddress(public_id) {
    getLocalitiesDetails(public_id).then((addressDetails) => {
        debugger;
        if (addressDetails.result["addresses"]) {
            populateAddressList(addressDetails.result["addresses"]);
        }else if (addressDetails) {
            fillAddressFields(addressDetails.result.address_components);
        }
    });
}

function populateAddressList(addresses) {
    if (inputElement && suggestionsList) {
        suggestionsList.innerHTML = "";
        if (addresses["list"].length > 0) {
            addresses["list"].forEach((address) => {
                let _a;
                const li = document.createElement("li");

                li.innerHTML =
                    (_a = formatPredictionList(address)) !== null && _a !== void 0
                        ? _a
                        : "";
                li.addEventListener("click", () => {
                    let _a;

                    inputElement.value =
                        (_a = address.description) !== null && _a !== void 0 ? _a : "";
                    requestDetailsAddress(address.public_id);
                });
                suggestionsList.appendChild(li);
            });
            suggestionsList.style.display = "block";
            clearSearchBtn.style.display = "block";
        }
    }
}

function getLocalitiesDetails(publicId) {
    const args = {
        public_id: publicId,
        key: woosmap_key,
    };
    return fetch(
        `https://api.woosmap.com/localities/details/?${buildQueryString(args)}`,
    ).then((response) => response.json());
}

function debouncePromise(fn, delay) {
    let timeoutId = null;
    let latestResolve = null;
    let latestReject = null;

    return function (...args) {
        return new Promise((resolve, reject) => {
            if (timeoutId !== null) {
                clearTimeout(timeoutId);
            }

            latestResolve = resolve;
            latestReject = reject;
            timeoutId = setTimeout(() => {
                fn(...args)
                    .then((result) => {
                        if (latestResolve === resolve && latestReject === reject) {
                            resolve(result);
                        }
                    })
                    .catch((error) => {
                        if (latestResolve === resolve && latestReject === reject) {
                            reject(error);
                        }
                    });
            }, delay);
        });
    };
}

function fillAddressFields(_data) {
    let _countyArray = _data.filter(function (item) {
        return item.types.includes("county");
    });
    if (_countyArray != undefined && _countyArray.length > 0) {
        $('#County').val(_countyArray[0].long_name);
    }

    let _localityArray = _data.filter(function (item) {
        return item.types.includes("locality");
    });
    if (_localityArray != undefined && _localityArray.length > 0) {
        $('#City').val(_localityArray[0].long_name);
    }


    let _postCodeArray = _data.filter(function (item) {
        return item.types.includes("postal_codes");
    });
    if (_postCodeArray != undefined && _postCodeArray.length > 0) {
        $('#Postcode').val(_postCodeArray[0].long_name);
    }

    let _organisationArray = _data.filter(function (item) {
        return item.types.includes("organisation");
    });

    if (_organisationArray != undefined && _organisationArray.length > 0) {
        $('#CompanyName').val(_organisationArray[0].long_name);
    }

    let _countryArray = _data.filter(function (item) {
        return item.types.includes("country");
    });

    if (_countryArray != undefined && _countryArray.length > 0) {
        $('#Country').val(_countryArray[0].long_name);
    }

    let _routeArray = _data.filter(function (item) {
        return item.types.includes("route");
    });


    let _houseArray = _data.filter(function (item) {
        return item.types.includes("premise");
    });

    let _streetArray = _data.filter(function (item) {
        return item.types.includes("street_number");
    });


    let _address = "";

    if (_houseArray != undefined && _houseArray.length > 0) {
        _address = _houseArray[0].long_name + ', ';
    }

    if (_streetArray != undefined && _streetArray.length > 0) {
        _address += _streetArray[0].long_name + ', ';
    }
    if (_routeArray != undefined && _routeArray.length > 0) {
        _address += _routeArray[0].long_name;
    }
    $('#AddressLineOne').val(_address);
    $('#suggestions-list').hide();
}
document.addEventListener("DOMContentLoaded", () => {
    inputElement = document.getElementById("autocomplete-input");
    suggestionsList = document.getElementById("suggestions-list");
    clearSearchBtn = document.getElementsByClassName("clear-searchButton")[0];
    init();
});

//$('#autocomplete-input').focusin(function () {
//    suggestionsList.style.display = "block";
//    clearSearchBtn.style.display = "block";
//});

//// When the textbox loses focus
//$('#autocomplete-input').focusout(function () {
//    suggestionsList.style.display = "none";
//    clearSearchBtn.style.display = "none";
//});



$(document).ready(function () {
    $('#autocomplete-input').click(function () {
        $('#suggestions-list').show();
    });

    //$(document).click(function (event) {
    //    var target = $(event.target);
    //    if (!target.closest('#autocomplete-input').length && !target.closest('#suggestions-list').length) {
    //        $('#suggestions-list').hide();
    //    }
    //});
});
